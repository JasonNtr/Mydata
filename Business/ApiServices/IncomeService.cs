using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Resources;
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
    public class IncomeService : IIncomeService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IIncomeRepo _incomeRepo;
        private readonly IMyDataIncomeResponseRepo _myDataIncomeResponseRepo;

        public IncomeService(IOptions<AppSettings> appSettings, IIncomeRepo incomeRepo, IMyDataIncomeResponseRepo myDataIncomeResponseRepo)
        {
            _appSettings = appSettings;
            _incomeRepo = incomeRepo;
            _myDataIncomeResponseRepo = myDataIncomeResponseRepo;
        }

        public MyDataIncomeResponseDTO ParseIncomePostResult(string httpResponseContext)
        {
            var mydataresponse = new MyDataIncomeResponseDTO();
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(httpResponseContext);
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
                mydataresponse.Index = index;
                mydataresponse.IncomeUid = invoiceUid;
                mydataresponse.IncomeMark = invoiceMark;
                mydataresponse.AuthenticationCode = authenticationCode;
                mydataresponse.StatusCode = statusCode;

                if (errorsnode != null)
                {
                    foreach (XmlNode node in errorsnode)
                    {
                        var message = node.SelectSingleNode("message")?.InnerText;
                        var code = Convert.ToInt32(node.SelectSingleNode("code")?.InnerText);
                        var myDataError = new MyDataIncomeErrorDTO()
                        {
                            Id = Guid.NewGuid(),
                            Created = DateTime.UtcNow,
                            Modified = DateTime.UtcNow,
                            Message = message,
                            Code = code,
                            MyDataIncomeResponseId = mydataresponse.Id
                        };
                        mydataresponse.Errors.Add(myDataError);
                    }
                }

                return mydataresponse;
            }
            catch (Exception ex)
            {
                mydataresponse.StatusCode = "Program Error";
                mydataresponse.Errors.Add(
                    new MyDataIncomeErrorDTO()
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

        public async Task<int> PostIncome(string incomeFilePath)
        {
            var myDataIncomeDTO = await BuildIncome(incomeFilePath);

            var url = _appSettings.Value.url;
            var aadeUserId = _appSettings.Value.aade_user_id;
            var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;
            var client = new HttpClient();

            var invoiceXmlFile = "";
            try
            {
                invoiceXmlFile = await File.ReadAllTextAsync(incomeFilePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("PostInvoice " + ex);
            }

            // Request headers
            client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
            var uri = url + "/SendIncomeClassification"; // + queryString;
            var byteData = Encoding.UTF8.GetBytes(invoiceXmlFile);
            using var content = new ByteArrayContent(byteData);
            content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
            var httpResponseMessage = await client.PostAsync(uri, content);
            if (!httpResponseMessage.IsSuccessStatusCode)
                return 0;

            var httpResponseContext = await httpResponseMessage.Content.ReadAsStringAsync();
            var myDataIncomeResponse = ParseIncomePostResult(httpResponseContext);
            if (myDataIncomeDTO != null && myDataIncomeResponse != null)
            {
                myDataIncomeResponse.MyDataIncomeId = myDataIncomeDTO.Id;
                await _myDataIncomeResponseRepo.Insert(myDataIncomeResponse);
                myDataIncomeDTO.MyDataIncomeResponses.Add(myDataIncomeResponse);
                //await _invoiceRepo.AddResponses(myDataInvoiceDTO);//, mydataresponse);
            }
            Debug.WriteLine("Post Income Completed");
            return 0;
        }

        public async Task<MyDataIncomeDTO> BuildIncome(string filenamePath)
        {
            // Here we parse the file using its name to create a Income base on the file name and only ->
            var filename = Path.GetFileName(filenamePath);
            //format 20210204-0000124-INV0001-038644960-00000000001.xml
            var provider = new CultureInfo("en-US");
            var fileNameParts = filename.Split('-');
            if (fileNameParts.Length < 5)
                return null;

            DateTime? datetime = DateTime.Now;
            const string format = "yyyyMMdd";

            #region FileNameParsing

            //try
            //{
            //    datetime = DateTime.ParseExact(fileNameParts[0], format, provider);
            //    Console.WriteLine("{0} converts to {1}.", fileNameParts[0], datetime.ToString());
            //}
            //catch (FormatException)
            //{
            //    Console.WriteLine("{0} is not in the correct format.", fileNameParts[0]);
            //}

            long? invoiceNumber = null;
            //try
            //{
            //    invoiceNumber = long.Parse(fileNameParts[1]);
            //    Console.WriteLine("{0} converts to {1}.", fileNameParts[1], invoiceNumber);
            //}
            //catch (FormatException)
            //{
            //    Console.WriteLine("{0} is not in the correct format.", fileNameParts[1]);
            //}

            long? uid = null;
            //try
            //{
            //    uid = Int64.Parse(fileNameParts[4].Replace(".xml", ""));
            //    Console.WriteLine("{0} converts to {1}.", fileNameParts[4], uid);
            //}
            //catch (FormatException)
            //{
            //    Console.WriteLine("{0} is not in the correct format.", fileNameParts[4]);
            //}

            int type = 0;
            //var parseType = fileNameParts[2].Replace("INV", "");
            //try
            //{
            //    type = Int32.Parse(parseType);
            //    Console.WriteLine("{0} converts to {1}.", parseType, type);
            //}
            //catch (FormatException)
            //{
            //    Console.WriteLine("{0} is not in the correct format.", parseType);
            //}

            long? cancellationMark = null;
            //if (fileNameParts.Length == 6)
            //{
            //    var parseCancellationMark = fileNameParts[5].Replace(".xml", "");
            //    try
            //    {
            //        cancellationMark = long.Parse(parseCancellationMark);
            //        Console.WriteLine("{0} converts to {1}.", parseCancellationMark, cancellationMark);
            //    }
            //    catch (FormatException)
            //    {
            //        Console.WriteLine("{0} is not in the correct format.", parseCancellationMark);
            //    }
            //}

            #endregion


            MyDataIncomeDTO myDataIncomeDTO;
            var exist = await _incomeRepo.ExistedUid(uid);
            if (exist)
            {
                myDataIncomeDTO = await _incomeRepo.GetByUid(uid);
                myDataIncomeDTO.IncomeDate = datetime;
                //MyDataInvoiceDTO.InvoiceType = null;
                myDataIncomeDTO.IncomeTypeCode = type;
                myDataIncomeDTO.FileName = filename;
                myDataIncomeDTO.StoredXml = ""; //myDataInvoiceDTO.StoredXml;
                myDataIncomeDTO.IncomeNumber = invoiceNumber;
                myDataIncomeDTO.Modified = DateTime.Now;
                myDataIncomeDTO.VAT = "6666";
                //myDataIncomeDTO.VAT = fileNameParts[3];
                myDataIncomeDTO = await _incomeRepo.Update(myDataIncomeDTO);
            }
            else
            {
                myDataIncomeDTO = new MyDataIncomeDTO()
                {
                    Id = Guid.NewGuid(),
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    Uid = 1313, //uid
                    FileName = filename,
                    StoredXml = "",//AppSettings.Value.folderPath + "/Income/Stored/" + filename,
                    IncomeDate = datetime,
                    IncomeTypeCode = type,
                    IncomeNumber = invoiceNumber,
                    //VAT = fileNameParts[3],
                    VAT = "13131",
                };
                myDataIncomeDTO = await _incomeRepo.Insert(myDataIncomeDTO);
            }

            return myDataIncomeDTO;
        }
    }
}
