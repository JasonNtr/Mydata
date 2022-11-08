using AutoMapper;
using Business.Mappings;
using Business.Services;
using Domain.AADE;
using Domain.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Business.ApiServices
{
    public class InvoiceService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly string _connectionString;

        public InvoiceService(IOptions<AppSettings> appSettings, string connectionString)
        {
            _appSettings = appSettings;
            _connectionString = connectionString;
        }

        public async Task<bool> PostActionNew(MyDataInvoiceTransferModel transferModel)
        {
            var url = _appSettings.Value.url;
            var aadeUserId = _appSettings.Value.aade_user_id;
            var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;

            var uri = url + "/SendInvoices"; // + queryString;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
            HttpResponseMessage httpResponseMessage;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes(transferModel.Xml);
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                httpResponseMessage = await client.PostAsync(uri, content);
            }
            var httpresponsecontext = await httpResponseMessage.Content.ReadAsStringAsync();
            var mydataresponse = ParseInvoiceResponseResult(httpresponsecontext);

            var i = 0;
            var particleRepo = new ParticleRepo(_connectionString);
            var result = false;
            foreach (var invoice in transferModel.MyDataInvoices)
            {
                mydataresponse[i].MyDataInvoiceId = invoice.Id;
                invoice.MyDataResponses.Add(mydataresponse[i]);
                if (mydataresponse[i].statusCode.Equals("Success"))
                {
                    invoice.Particle.Mark = mydataresponse[i].invoiceMark.ToString();
                    result = await particleRepo.Update(invoice.Particle);
                }
                i++;
            }

            var invoiceRepo = new InvoiceRepo(_connectionString);

            result = await invoiceRepo.InsertOrUpdateRangeForPost(transferModel.MyDataInvoices);

            return result;
        }

        public async Task<bool> CancelActionNew(MyDataInvoiceTransferModel transferModel)
        {
            var url = _appSettings.Value.url;
            var aadeUserId = _appSettings.Value.aade_user_id;
            var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
            var result = false;

            var byteData = new byte[0];
            using var content = new ByteArrayContent(byteData);

            var httpResponseMessage = string.Empty;
            foreach (var invoice in transferModel.MyDataInvoices)
            {
                var uri = url + "/CancelInvoice?mark=" + invoice.CancellationMark.ToString();
                var httpResponse = await client.PostAsync(uri, content);
                httpResponseMessage = await httpResponse.Content.ReadAsStringAsync();

                var mydataresponse = ParseInvoiceCancelResponseResult(httpResponseMessage);

                var particleRepo = new ParticleRepo(_connectionString);

                mydataresponse.MyDataInvoiceId = invoice.Id;
                invoice.MyDataCancelationResponses.Add(mydataresponse);
                if (mydataresponse.statusCode.Equals("Success"))
                {
                    invoice.Particle.Mark = mydataresponse.cancellationMark.ToString();
                    result = await particleRepo.Update(invoice.Particle);

                    var particleToBeCancelled = await particleRepo.GetByMark(invoice.CancellationMark);
                    particleToBeCancelled.CancelMark = invoice.Particle.Mark;
                    result = await particleRepo.Update(particleToBeCancelled);
                }
            }

            var invoiceRepo = new InvoiceRepo(_connectionString);

            result = await invoiceRepo.InsertOrUpdateRangeForCancel(transferModel.MyDataInvoices);

            return false;
        }

        private MyDataCancelationResponseDTO ParseInvoiceCancelResponseResult(string httpresponsecontext)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(httpresponsecontext);

                XmlSerializer mySerializer = new XmlSerializer(typeof(ResponseDoc));

                var myResponseData = new ResponseDoc();
                using (TextReader reader = new StringReader(xmlDoc.InnerXml))
                {
                    myResponseData = (ResponseDoc)mySerializer.Deserialize(reader);
                }

                var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfiles()); });
                var mapper = new AutoMapper.Mapper(mapperConfig);
                var mydataresponse = mapper.Map<List<MyDataCancelationResponseDTO>>(myResponseData.response);

                return mydataresponse.FirstOrDefault();
            }
            catch (Exception ex)
            {
                var mydataresponse = new MyDataCancelationResponseDTO();
                mydataresponse.statusCode = "Program Error";
                mydataresponse.errors.Add(
                    new MyDataCancelationErrorDTO()
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.UtcNow,
                        Modified = DateTime.UtcNow,
                        message = ex.ToString(),
                        code = 0
                    });

                return mydataresponse;
            }

            return null;
        }

        private List<MyDataResponseDTO> ParseInvoiceResponseResult(string httpresponsecontext)
        {
            var mydataresponses = new List<MyDataResponseDTO>();
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(httpresponsecontext);

                XmlSerializer mySerializer = new XmlSerializer(typeof(ResponseDoc));

                var myResponseData = new ResponseDoc();
                using (TextReader reader = new StringReader(xmlDoc.InnerXml))
                {
                    myResponseData = (ResponseDoc)mySerializer.Deserialize(reader);
                }

                foreach (var response in myResponseData.response)
                {
                    var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfiles()); });
                    var mapper = new AutoMapper.Mapper(mapperConfig);
                    var mydataresponse = mapper.Map<MyDataResponseDTO>(response);
                    mydataresponses.Add(mydataresponse);
                }

                return mydataresponses;
            }
            catch (Exception ex)
            {
                var mydataresponse = new MyDataResponseDTO();
                mydataresponse.statusCode = "Program Error";
                mydataresponse.errors.Add(
                    new MyDataErrorDTO()
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.UtcNow,
                        Modified = DateTime.UtcNow,
                        message = ex.ToString(),
                        code = 0
                    });
                mydataresponses.Add(mydataresponse);
                return mydataresponses;
            }

            return null;
        }
    }
}