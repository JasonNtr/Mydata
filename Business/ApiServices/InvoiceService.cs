using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

using Domain.DTO;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Business.ApiServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IOptions<AppSettings> AppSettings;
        private readonly IInvoiceRepo invoiceRepo;
        private readonly IMyDataResponseRepo mydataresponseRepo;
        private readonly IMyDataCancellationResponseRepo mydatacancelationresponseRepo;
        private readonly IParticleInform particleInform;

        public InvoiceService(IOptions<AppSettings> AppSettings, IInvoiceRepo invoiceRepo, IMyDataResponseRepo mydataresponseRepo, IMyDataCancellationResponseRepo mydatacancelationresponseRepo, IParticleInform particleInform)
        {
            this.AppSettings = AppSettings;
            this.invoiceRepo = invoiceRepo;
            this.mydataresponseRepo = mydataresponseRepo;
            this.mydatacancelationresponseRepo = mydatacancelationresponseRepo;
            this.particleInform = particleInform;
        }
        public async Task<int> PostAction(string file_path)
        {
            var mydatainvoicedto = await BuildInvoice(file_path);

            var result = 0;
            if (mydatainvoicedto.InvoiceTypeCode == 215)
            {
                result = await CancelInvoice(mydatainvoicedto);
            }
            else
            {
                result = await PostInvoice(mydatainvoicedto, file_path);
            }

            return result;
        }

        public async Task<int> PostInvoice(MyDataInvoiceDTO mydatainvoicedto, string invoice_file_path)
        {
            var url = AppSettings.Value.url;
            var aade_user_id = AppSettings.Value.aade_user_id;
            var Ocp_Apim_Subscription_Key = AppSettings.Value.Ocp_Apim_Subscription_Key;
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var invoice_xml_file = "";
            try
            {
                invoice_xml_file = await File.ReadAllTextAsync(invoice_file_path);
            }
            catch (Exception ex)
            {

            }

            // Request headers
            client.DefaultRequestHeaders.Add("aade-user-id", aade_user_id);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Ocp_Apim_Subscription_Key);
            var uri = url + "/SendInvoices"; // + queryString;
            HttpResponseMessage httpresponse;
            byte[] byteData = Encoding.UTF8.GetBytes(invoice_xml_file);
            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                httpresponse = await client.PostAsync(uri, content);
                if (httpresponse.IsSuccessStatusCode)
                {
                    var httpresponsecontext = await httpresponse.Content.ReadAsStringAsync();
                    var mydataresponse = ParseInvoicePostResult(httpresponsecontext);
                    if (mydatainvoicedto != null && mydataresponse != null)
                    {
                        mydataresponse.MyDataInvoiceId = mydatainvoicedto.Id;
                        await mydataresponseRepo.Insert(mydataresponse);
                        mydatainvoicedto.MyDataResponses.Add(mydataresponse);
                        await invoiceRepo.AddResponses(mydatainvoicedto);//, mydataresponse);

                        if (mydataresponse.statusCode.Equals("Success"))
                            await particleInform.UpdateParticle(mydatainvoicedto);
                    }
                }
            }

            return 0;
        }

        public async Task<int> CancelInvoice(MyDataInvoiceDTO mydatainvoicedto)
        {

            var url = AppSettings.Value.url;
            var aade_user_id = AppSettings.Value.aade_user_id;
            var Ocp_Apim_Subscription_Key = AppSettings.Value.Ocp_Apim_Subscription_Key;
            var client = new HttpClient();
           
            var result = 0;
            // Request headers
            client.DefaultRequestHeaders.Add("aade-user-id", aade_user_id);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Ocp_Apim_Subscription_Key);
           
            var uri = url + "/CancelInvoice?mark=" + mydatainvoicedto.CancellationMark.ToString(); // + queryString;
            HttpResponseMessage httpresponse;
            byte[] byteData = new byte[0];
            using (var content = new ByteArrayContent(byteData))
            {
                //content.Headers.Add("mark", mydatainvoicedto.CancellationMark.ToString());
               
                httpresponse = await client.PostAsync(uri, content);
                if (httpresponse.IsSuccessStatusCode)
                {
                    var httpresponsecontext = await httpresponse.Content.ReadAsStringAsync();

                    var mydataCancellationresponse = ParseCancellationResponseResult(httpresponsecontext);
                    if (mydatainvoicedto != null && mydataCancellationresponse != null)
                    {
                        mydataCancellationresponse.MyDataInvoiceId = mydatainvoicedto.Id;
                        await mydatacancelationresponseRepo.Insert(mydataCancellationresponse);
                        mydatainvoicedto.MyDataCancelationResponses.Add(mydataCancellationresponse);
                        await invoiceRepo.AddResponses(mydatainvoicedto);//, mydataresponse);

                        if (mydataCancellationresponse.statusCode.Equals("Success"))
                        {
                            var mydatainvoicedtothatcancelled =
                                await invoiceRepo.GetByMark(mydatainvoicedto.CancellationMark.Value);
                            await particleInform.UpdateCancellationParticle(mydatainvoicedto, mydatainvoicedtothatcancelled);


                        }

                        result = 1;
                    }
                }
            }

            return result;
        }

        public MyDataResponseDTO ParseInvoicePostResult(string httpresponsecontext)
        {
            var mydataresponse = new MyDataResponseDTO();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(httpresponsecontext);
                XmlNode ResponseDoc = xmlDoc.SelectSingleNode("ResponseDoc");
                XmlNode response = ResponseDoc.SelectSingleNode("response");
                XmlNode indexnode = response.SelectSingleNode("index");
                XmlNode invoiceUidnode = response.SelectSingleNode("invoiceUid");
                XmlNode invoiceMarknode = response.SelectSingleNode("invoiceMark");
                XmlNode authenticationCodenode = response.SelectSingleNode("authenticationCode");
                XmlNode statusCodenode = response.SelectSingleNode("statusCode");
                XmlNode errorsnodelist = response.SelectSingleNode("errors");
                XmlNodeList errorsnode = null;
                if (errorsnodelist != null)
                    errorsnode = errorsnodelist.SelectNodes("error");

                int? index = null;
                if (indexnode != null)
                {
                    index = Convert.ToInt32(indexnode.InnerText);
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
                        string message = node.SelectSingleNode("message").InnerText;
                        int code = Convert.ToInt32(node.SelectSingleNode("code").InnerText);
                        var mydataerror = new MyDataErrorDTO()
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                            Message = message,
                            Code = code, 
                            MyDataResponseId = mydataresponse.Id
                        };
                        mydataresponse.Errors.Add(mydataerror);
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

        public MyDataCancelationResponseDTO ParseCancellationResponseResult(string httpresponsecontext)
        {
            var mydataerrorresponse = new MyDataCancelationResponseDTO();
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(httpresponsecontext);
                XmlNode ResponseDoc = xmlDoc.SelectSingleNode("ResponseDoc");
                XmlNode response = ResponseDoc.SelectSingleNode("response");
                XmlNode cancellationMarknode = response.SelectSingleNode("cancellationMark");
                XmlNode statusCodenode = response.SelectSingleNode("statusCode");
                XmlNode errorsnodelist = response.SelectSingleNode("errors");
                XmlNodeList errorsnode = null;
                if (errorsnodelist != null)
                    errorsnode = errorsnodelist.SelectNodes("error");


                long? cancellationMark = null;
                if (cancellationMarknode != null)
                {
                    cancellationMark = Convert.ToInt64(cancellationMarknode.InnerText);
                }

                string statusCode = null;
                if (statusCodenode != null)
                {
                    statusCode = statusCodenode.InnerText;
                }

                mydataerrorresponse.Id = Guid.NewGuid();
                mydataerrorresponse.Created = DateTime.UtcNow;
                mydataerrorresponse.Modified = DateTime.UtcNow;
                mydataerrorresponse.cancellationMark = cancellationMark;
                mydataerrorresponse.statusCode = statusCode;

                if (errorsnode != null)
                {
                    foreach (XmlNode node in errorsnode)
                    {
                        string message = node.SelectSingleNode("message").InnerText;
                        int code = Convert.ToInt32(node.SelectSingleNode("code").InnerText);
                        var mydataerror = new MyDataCancelationErrorDTO()
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                            Message = message,
                            Code = code, 
                            MyDataCancelationResponseId = mydataerrorresponse.Id
                        };
                        mydataerrorresponse.Errors.Add(mydataerror);
                    }
                }

                return mydataerrorresponse;
            }
            catch (Exception ex)
            {
                mydataerrorresponse.statusCode = "Program Error";
                mydataerrorresponse.Errors.Add(
                    new MyDataCancelationErrorDTO()
                    {
                        Id = Guid.NewGuid(),
                        Created = DateTime.UtcNow,
                        Modified = DateTime.UtcNow,
                        Message = ex.ToString(),
                        Code = 0
                    });
                return mydataerrorresponse;
            }
        }

        public async Task<MyDataInvoiceDTO> BuildInvoice(string filenamepath)
        {

            var filename = Path.GetFileName(filenamepath);
            //format 20210204-0000124-INV0001-038644960-00000000001.xml
            var provider = new CultureInfo("en-US");
            var filenameparts = filename.Split('-');
            if (filenameparts.Length < 5)
                return null;
            DateTime? datetime = null;
            var format = "yyyyMMdd";
            
            try
            {
                datetime = DateTime.ParseExact(filenameparts[0], format, provider);
                Console.WriteLine("{0} converts to {1}.", filenameparts[0], datetime.ToString());
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", filenameparts[0]);
            }

            long? invoicenumber = null;
            try
            {
                invoicenumber = Int64.Parse(filenameparts[1]);
                Console.WriteLine("{0} converts to {1}.", filenameparts[1], invoicenumber);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", filenameparts[1]);
            }

            long? uid = null;
            try
            {
                uid = Int64.Parse(filenameparts[4].Replace(".xml", ""));
                Console.WriteLine("{0} converts to {1}.", filenameparts[4], uid);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", filenameparts[4]);
            }

            int type = 0;
            var parsetype = filenameparts[2].Replace("INV", "");
            try
            {
                type = Int32.Parse(parsetype);
                Console.WriteLine("{0} converts to {1}.", parsetype, type);
            }
            catch (FormatException)
            {
                Console.WriteLine("{0} is not in the correct format.", parsetype);
            }

            long? cancellationMark = null;
            if (filenameparts.Length == 6)
            {
                var parsecancellationMark = filenameparts[5].Replace(".xml", "") ;
                try
                {
                    cancellationMark = Int64.Parse(parsecancellationMark);
                    Console.WriteLine("{0} converts to {1}.", parsecancellationMark, cancellationMark);
                }
                catch (FormatException)
                {
                    Console.WriteLine("{0} is not in the correct format.", parsecancellationMark);
                }
            }

            MyDataInvoiceDTO MyDataInvoiceDTO;
            var exist = await invoiceRepo.ExistedUid(uid);
            if (exist)
            {
                MyDataInvoiceDTO = await invoiceRepo.GetByUid(uid);
                MyDataInvoiceDTO.InvoiceDate = datetime;
                //MyDataInvoiceDTO.InvoiceType = null;
                MyDataInvoiceDTO.InvoiceTypeCode = type;
                MyDataInvoiceDTO.FileName = filename;
                MyDataInvoiceDTO.StoredXml = ""; //mydatainvoicedto.StoredXml;
                MyDataInvoiceDTO.InvoiceNumber = invoicenumber;
                MyDataInvoiceDTO.Modified = DateTime.Now;
                MyDataInvoiceDTO.VAT = filenameparts[3];
                MyDataInvoiceDTO.CancellationMark = cancellationMark;
                MyDataInvoiceDTO = await invoiceRepo.Update(MyDataInvoiceDTO);
            }
            else
            {
                MyDataInvoiceDTO = new MyDataInvoiceDTO()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    Uid = uid,
                    FileName = filename,
                    StoredXml = "",//AppSettings.Value.folderPath + "/Invoice/Stored/" + filename,
                    InvoiceDate = datetime,
                    InvoiceTypeCode = type,
                    InvoiceNumber = invoicenumber,
                    VAT = filenameparts[3],
                    CancellationMark = cancellationMark
                };
                MyDataInvoiceDTO = await invoiceRepo.Insert(MyDataInvoiceDTO);
            }
            
            return MyDataInvoiceDTO;
        }

        
    }
}
