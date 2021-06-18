using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Domain.DTO;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Business.ApiServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IInvoiceRepo _invoiceRepo;
        private readonly IMyDataResponseRepo _myDataResponseRepo;
        private readonly IMyDataCancellationResponseRepo _myDataCancellationResponseRepo;
        private readonly IParticleInform _particleInform;

        public InvoiceService(IOptions<AppSettings> appSettings, IInvoiceRepo invoiceRepo, IMyDataResponseRepo myDataResponseRepo, IMyDataCancellationResponseRepo myDataCancellationResponseRepo, IParticleInform particleInform)
        {
            _appSettings = appSettings;
            _invoiceRepo = invoiceRepo;
            _myDataResponseRepo = myDataResponseRepo;
            _myDataCancellationResponseRepo = myDataCancellationResponseRepo;
            _particleInform = particleInform;
        }
        public async Task<int> PostAction(string filePath)
        {
            var myDataInvoiceDTO = await BuildInvoice(filePath);

            int result;
            if (myDataInvoiceDTO.InvoiceTypeCode == 215)
            {
                result = await CancelInvoice(myDataInvoiceDTO);
            }
            else
            {
                result = await PostInvoice(myDataInvoiceDTO, filePath);
            }

            return result;
        }

        public async Task<int> PostInvoice(MyDataInvoiceDTO myDataInvoiceDTO, string invoiceFilePath)
        {
            var url = _appSettings.Value.url;
            var aadeUserId = _appSettings.Value.aade_user_id;
            var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;
            var client = new HttpClient();

            var invoiceXmlFile = "";
            try
            {
                invoiceXmlFile = await File.ReadAllTextAsync(invoiceFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PostInvoice " + ex);
            }

            // Request headers
            client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
            var uri = url + "/SendInvoices"; // + queryString;
            var byteData = Encoding.UTF8.GetBytes(invoiceXmlFile);
            using var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            var httpResponseMessage = await client.PostAsync(uri, content);
            if (!httpResponseMessage.IsSuccessStatusCode) 
                return 0;
            var httpresponsecontext = await httpResponseMessage.Content.ReadAsStringAsync();
            var mydataresponse = ParseInvoicePostResult(httpresponsecontext);
            if (myDataInvoiceDTO != null && mydataresponse != null)
            {
                mydataresponse.MyDataInvoiceId = myDataInvoiceDTO.Id;
                await _myDataResponseRepo.Insert(mydataresponse);
                myDataInvoiceDTO.MyDataResponses.Add(mydataresponse);
                //await _invoiceRepo.AddResponses(myDataInvoiceDTO);//, mydataresponse);

                if (mydataresponse.statusCode.Equals("Success"))
                    await _particleInform.UpdateParticle(myDataInvoiceDTO);
            }
            Debug.WriteLine("Post Invoice Completed");
            return 0;
        }

        public async Task<int> CancelInvoice(MyDataInvoiceDTO myDataInvoiceDTO)
        {

            var url = _appSettings.Value.url;
            var aadeUserId = _appSettings.Value.aade_user_id;
            var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;
            var client = new HttpClient();
           
            var result = 0;
            // Request headers
            client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
           
            var uri = url + "/CancelInvoice?mark=" + myDataInvoiceDTO.CancellationMark.ToString(); // + queryString;
            var byteData = new byte[0];
            using var content = new ByteArrayContent(byteData);
            //content.Headers.Add("mark", myDataInvoiceDTO.CancellationMark.ToString());

            var httpResponse = await client.PostAsync(uri, content);
            if (!httpResponse.IsSuccessStatusCode) 
                return result;
            var httpResponseContext = await httpResponse.Content.ReadAsStringAsync();

            var myDataCancellationResponse = ParseCancellationResponseResult(httpResponseContext);
            if (myDataCancellationResponse == null) 
                return result;
            myDataCancellationResponse.MyDataInvoiceId = myDataInvoiceDTO.Id;
            await _myDataCancellationResponseRepo.Insert(myDataCancellationResponse);
            myDataInvoiceDTO.MyDataCancelationResponses.Add(myDataCancellationResponse);
            //await _invoiceRepo.AddResponses(myDataInvoiceDTO);//, mydataresponse);

            if (myDataCancellationResponse.statusCode.Equals("Success") &&
                myDataInvoiceDTO.CancellationMark != null)
            {
                var myDataInvoiceDTOThatCancelled =
                    await _invoiceRepo.GetByMark(myDataInvoiceDTO.CancellationMark.Value);
                await _particleInform.UpdateCancellationParticle(myDataInvoiceDTO,
                    myDataInvoiceDTOThatCancelled);
            }

            result = 1;

            return result;
        }

        public MyDataResponseDTO ParseInvoicePostResult(string httpresponsecontext)
        {
            var mydataresponse = new MyDataResponseDTO();
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(httpresponsecontext);
                var responseDoc = xmlDoc.SelectSingleNode("ResponseDoc");
                var response = responseDoc?.SelectSingleNode("response");
                var indexNode = response?.SelectSingleNode("index");
                var invoiceUidnode = response?.SelectSingleNode("invoiceUid");
                var invoiceMarknode = response?.SelectSingleNode("invoiceMark");
                var authenticationCodenode = response?.SelectSingleNode("authenticationCode");
                var statusCodenode = response?.SelectSingleNode("statusCode");
                var errorsnodelist = response?.SelectSingleNode("errors");
                XmlNodeList errorsnode = null;
                if (errorsnodelist != null)
                    errorsnode = errorsnodelist.SelectNodes("error");

                int? index = null;
                if (indexNode != null)
                {
                    index = Convert.ToInt32(indexNode.InnerText);
                }

                string invoiceUid = null;
                if (invoiceUidnode != null)
                {
                    invoiceUid = invoiceUidnode.InnerText;
                }

                long? invoiceMark = null;
                if (invoiceMarknode != null)
                {
                    invoiceMark = Convert.ToInt64(invoiceMarknode.InnerText);
                }

                string authenticationCode = null;
                if (authenticationCodenode != null)
                {
                    authenticationCode = authenticationCodenode.InnerText;
                }

                string statusCode = null;
                if (statusCodenode != null)
                {
                    statusCode = statusCodenode.InnerText;
                }


                mydataresponse.Id = Guid.NewGuid();
                mydataresponse.Created = DateTime.UtcNow;
                mydataresponse.Modified = DateTime.UtcNow;
                mydataresponse.index = index;
                mydataresponse.invoiceUid = invoiceUid;
                mydataresponse.invoiceMark = invoiceMark;
                mydataresponse.authenticationCode = authenticationCode;
                mydataresponse.statusCode = statusCode;

                if (errorsnode != null)
                {
                    foreach (XmlNode node in errorsnode)
                    {
                        var message = node.SelectSingleNode("message")?.InnerText;
                        var code = Convert.ToInt32(node.SelectSingleNode("code")?.InnerText);
                        var myDataError = new MyDataErrorDTO()
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                            Message = message,
                            Code = code, 
                            MyDataResponseId = mydataresponse.Id
                        };
                        mydataresponse.Errors.Add(myDataError);
                    }
                }

                return mydataresponse;
            }
            catch (Exception ex)
            {
                mydataresponse.statusCode = "Program Error";
                mydataresponse.Errors.Add(
                    new MyDataErrorDTO()
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.UtcNow,
                        Modified = DateTime.UtcNow,
                        Message = ex.ToString(),
                        Code = 0
                    });
                return mydataresponse;
            }
        }

        public MyDataCancelationResponseDTO ParseCancellationResponseResult(string httpResponseContext)
        {
            var myDataErrorResponse = new MyDataCancelationResponseDTO();
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(httpResponseContext);
                var responseDoc = xmlDoc.SelectSingleNode("ResponseDoc");
                var response = responseDoc?.SelectSingleNode("response");
                var cancellationMarkNode = response?.SelectSingleNode("cancellationMark");
                var statusCodeNode = response?.SelectSingleNode("statusCode");
                var errorsNodeList = response?.SelectSingleNode("errors");
                XmlNodeList errorsNode = null;
                if (errorsNodeList != null)
                    errorsNode = errorsNodeList.SelectNodes("error");


                long? cancellationMark = null;
                if (cancellationMarkNode != null)
                {
                    cancellationMark = Convert.ToInt64(cancellationMarkNode.InnerText);
                }

                string statusCode = null;
                if (statusCodeNode != null)
                {
                    statusCode = statusCodeNode.InnerText;
                }

                myDataErrorResponse.Id = Guid.NewGuid();
                myDataErrorResponse.Created = DateTime.UtcNow;
                myDataErrorResponse.Modified = DateTime.UtcNow;
                myDataErrorResponse.cancellationMark = cancellationMark;
                myDataErrorResponse.statusCode = statusCode;

                if (errorsNode != null)
                {
                    foreach (XmlNode node in errorsNode)
                    {
                        var message = node.SelectSingleNode("message")?.InnerText;
                        var code = Convert.ToInt32(node.SelectSingleNode("code")?.InnerText);
                        var myDataError = new MyDataCancelationErrorDTO()
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                            Message = message,
                            Code = code, 
                            MyDataCancelationResponseId = myDataErrorResponse.Id
                        };
                        myDataErrorResponse.Errors.Add(myDataError);
                    }
                }

                return myDataErrorResponse;
            }
            catch (Exception ex)
            {
                myDataErrorResponse.statusCode = "Program Error";
                myDataErrorResponse.Errors.Add(
                    new MyDataCancelationErrorDTO()
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.UtcNow,
                        Modified = DateTime.UtcNow,
                        Message = ex.ToString(),
                        Code = 0
                    });
                return myDataErrorResponse;
            }
        }

        public async Task<MyDataInvoiceDTO> BuildInvoice(string filenamePath)
        {

            var filename = Path.GetFileName(filenamePath);
            //format 20210204-0000124-INV0001-038644960-00000000001.xml
            var provider = new CultureInfo("en-US");
            var fileNameParts = filename.Split('-');
            if (fileNameParts.Length < 5)
                return null;
            DateTime? datetime = null;
            const string format = "yyyyMMdd";
            
            try
            {
                datetime = DateTime.ParseExact(fileNameParts[0], format, provider);
                Console.WriteLine("{0} converts to {1}.", fileNameParts[0], datetime.ToString());
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", fileNameParts[0]);
            }

            long? invoiceNumber = null;
            try
            {
                invoiceNumber = long.Parse(fileNameParts[1]);
                Console.WriteLine("{0} converts to {1}.", fileNameParts[1], invoiceNumber);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", fileNameParts[1]);
            }

            long? uid = null;
            try
            {
                uid = Int64.Parse(fileNameParts[4].Replace(".xml", ""));
                Console.WriteLine("{0} converts to {1}.", fileNameParts[4], uid);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", fileNameParts[4]);
            }

            int type = 0;
            var parseType = fileNameParts[2].Replace("INV", "");
            try
            {
                type = Int32.Parse(parseType);
                Console.WriteLine("{0} converts to {1}.", parseType, type);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", parseType);
            }

            long? cancellationMark = null;
            if (fileNameParts.Length == 6)
            {
                var parseCancellationMark = fileNameParts[5].Replace(".xml", "") ;
                try
                {
                    cancellationMark = long.Parse(parseCancellationMark);
                    Console.WriteLine("{0} converts to {1}.", parseCancellationMark, cancellationMark);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not in the correct format.", parseCancellationMark);
                }
            }

            MyDataInvoiceDTO myDataInvoiceDTO;
            var exist = await _invoiceRepo.ExistedUid(uid);
            if (exist)
            {
                myDataInvoiceDTO = await _invoiceRepo.GetByUid(uid);
                myDataInvoiceDTO.InvoiceDate = datetime;
                //MyDataInvoiceDTO.InvoiceType = null;
                myDataInvoiceDTO.InvoiceTypeCode = type;
                myDataInvoiceDTO.FileName = filename;
                myDataInvoiceDTO.StoredXml = ""; //myDataInvoiceDTO.StoredXml;
                myDataInvoiceDTO.InvoiceNumber = invoiceNumber;
                myDataInvoiceDTO.Modified = DateTime.Now;
                myDataInvoiceDTO.VAT = fileNameParts[3];
                myDataInvoiceDTO.CancellationMark = cancellationMark;
                myDataInvoiceDTO = await _invoiceRepo.Update(myDataInvoiceDTO);
            }
            else
            {
                myDataInvoiceDTO = new MyDataInvoiceDTO()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    Uid = uid,
                    FileName = filename,
                    StoredXml = "",//AppSettings.Value.folderPath + "/Invoice/Stored/" + filename,
                    InvoiceDate = datetime,
                    InvoiceTypeCode = type,
                    InvoiceNumber = invoiceNumber,
                    VAT = fileNameParts[3],
                    CancellationMark = cancellationMark
                };
                myDataInvoiceDTO = await _invoiceRepo.Insert(myDataInvoiceDTO);
            }
            
            return myDataInvoiceDTO;
        }

        
    }
}
