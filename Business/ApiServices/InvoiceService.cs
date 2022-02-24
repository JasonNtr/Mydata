using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Domain.DTO;
using Infrastructure.Database.RequestDocModels;
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
        private readonly IMyDataTransmittedDocInvoicesRepo _transmittedDocRepo;

        public InvoiceService(IOptions<AppSettings> appSettings, IInvoiceRepo invoiceRepo, IMyDataResponseRepo myDataResponseRepo, IMyDataCancellationResponseRepo myDataCancellationResponseRepo, IParticleInform particleInform, IMyDataTransmittedDocInvoicesRepo myDataTransmittedDocInvoicesRepo)
        {
            _appSettings = appSettings;
            _invoiceRepo = invoiceRepo;
            _myDataResponseRepo = myDataResponseRepo;
            _myDataCancellationResponseRepo = myDataCancellationResponseRepo;
            _particleInform = particleInform;
            _transmittedDocRepo = myDataTransmittedDocInvoicesRepo;
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

        public async Task<int> RequestDocs(string mark)
        {
            ContinuationToken continuationToken = null;
            do
            {
                var url = _appSettings.Value.url;
                var aadeUserId = _appSettings.Value.aade_user_id;
                var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;
                var client = new HttpClient();

                var result = 0;
                // Request headers
                client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
                var nextPartitionKey = (continuationToken == null ? "" : continuationToken.nextPartitionKey);
                var nextRowKey = (continuationToken == null ? "" : continuationToken.nextRowKey);
                var uri = url + "/RequestDocs?mark=" + 0 + "&nextPartitionKey=" + nextPartitionKey + "&nextRowKey=" + nextRowKey;
                //var uri = url + "/RequestDocs?mark=" + 0; // + queryString;
                //var uri = url + "/RequestTransmittedDocs?mark=" + 0; // + queryString;
                var byteData = new byte[0];
                using var content = new ByteArrayContent(byteData);
                //content.Headers.Add("mark", myDataInvoiceDTO.CancellationMark.ToString());

                var httpResponse = await client.GetAsync(uri);
                if (!httpResponse.IsSuccessStatusCode)
                    return result;
                var httpResponseContext = await httpResponse.Content.ReadAsStringAsync();
                httpResponseContext = httpResponseContext.Replace("icls:", "");

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(httpResponseContext);
                var invoices = DeserializeXml(doc);

                //this is to pass the values of the requestedDoc to the DTO in order for them to be saved
                continuationToken = await ConvertRequestedDocsToDTO(invoices);
            } while (continuationToken != null);
            
            ////////if (continuationToken != null)
            ////////{
            ////////    uri = url +"/RequestDocs?mark=" + 0 + "&nextPartitionKey=" + continuationToken.nextPartitionKey + "&nextRowKey=" + continuationToken.nextRowKey;
            ////////    result = 0;
            ////////    client = new HttpClient();
            ////////    client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            ////////    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
            ////////    byteData = new byte[0];
            ////////    using var content2 = new ByteArrayContent(byteData);
            ////////    httpResponse = await client.GetAsync(uri);
            ////////    if (!httpResponse.IsSuccessStatusCode)
            ////////        return result;
            ////////    httpResponseContext = await httpResponse.Content.ReadAsStringAsync();
            ////////    httpResponseContext = httpResponseContext.Replace("icls:", "");
            ////////    doc = new XmlDocument();
            ////////    doc.LoadXml(httpResponseContext);
            ////////    invoices = DeserializeXml(doc);
            ////////    continuationToken = await ConvertRequestedDocsToDTO(invoices);
            ////////}
            //XmlSerializer mySerializer = new XmlSerializer(typeof(MyDataDocInvoiceDTO));
            //StreamReader myStreamReader = new StreamReader(@"C:\Users\Aris\Desktop\test.xml");
            //var myResponseData = (RequestedDoc)mySerializer.Deserialize(myStreamReader);
            return 1;
        }

        private async Task<ContinuationToken> ConvertRequestedDocsToDTO(RequestedDoc requestedDocs)
        {
            int cnt = 0;
            List<MyDataTransmittedDocInvoiceDTO> transmittedDocs = new List<MyDataTransmittedDocInvoiceDTO>();
            foreach (Invoice docInvoice in requestedDocs.invoicesDoc.invoice)
            {
                Debug.WriteLine(cnt);
                cnt++;
                MyDataTransmittedDocInvoiceDTO transmittedDoc = new MyDataTransmittedDocInvoiceDTO();
                transmittedDoc.Id = docInvoice.Id;
                transmittedDoc.Created = docInvoice.Created;
                transmittedDoc.Modified = docInvoice.Modified;
                transmittedDoc.Uid = docInvoice.Uid;
                transmittedDoc.mark = docInvoice.mark;
                transmittedDoc.authenticationCode = docInvoice.authenticationCode;

                if (docInvoice.issuer.Count > 0)
                {
                    transmittedDoc.issuer = new List<MyDataPartyTypeDTO> {
                    new MyDataPartyTypeDTO {
                        branch = docInvoice.issuer[0].branch,
                        country = docInvoice.issuer[0].country,
                        vatNumber = docInvoice.issuer[0].vatNumber,
                        Id = docInvoice.issuer[0].Id,
                        Created = docInvoice.issuer[0].Created,
                        Modified = docInvoice.issuer[0].Modified,
                        MyDataDocIssuerInvoiceId = docInvoice.Id } };
                }

                if (docInvoice.counterpart.Count > 0)
                {
                    if (docInvoice.counterpart[0].address != null)
                    {
                        transmittedDoc.counterpart = new List<MyDataPartyTypeDTO> {
                        new MyDataPartyTypeDTO {branch = docInvoice.counterpart[0].branch,
                            country = docInvoice.counterpart[0].country,
                            vatNumber = docInvoice.counterpart[0].vatNumber,
                            address = new MyDataAddressTypeDTO {Id = docInvoice.counterpart[0].address.Id, Created = docInvoice.counterpart[0].address.Created, Modified = docInvoice.counterpart[0].address.Modified,
                                postalCode = docInvoice.counterpart[0].address.postalCode,
                                city = docInvoice.counterpart[0].address.city,
                                street = docInvoice.counterpart[0].address.street,
                                number = docInvoice.counterpart[0].address.number,
                                MyDataPartyTypeId = docInvoice.counterpart[0].Id},
                            Id = docInvoice.counterpart[0].Id,
                            Created = docInvoice.counterpart[0].Created,
                            Modified = docInvoice.counterpart[0].Modified,
                            MyDataDocEncounterInvoiceId = docInvoice.Id} };
                    }
                    else
                    {
                        transmittedDoc.counterpart = new List<MyDataPartyTypeDTO> {
                        new MyDataPartyTypeDTO {branch = docInvoice.counterpart[0].branch,
                            country = docInvoice.counterpart[0].country,
                            vatNumber = docInvoice.counterpart[0].vatNumber,
                            Id = docInvoice.counterpart[0].Id,
                            Created = docInvoice.counterpart[0].Created,
                            Modified = docInvoice.counterpart[0].Modified,
                            MyDataDocEncounterInvoiceId = docInvoice.Id} };
                    }
                    
                }

                transmittedDoc.invoiceHeaderType = new MyDataInvoiceHeaderTypeDTO {
                    Id = docInvoice.invoiceHeader.Id, Created = docInvoice.invoiceHeader.Created, Modified = docInvoice.invoiceHeader.Modified,
                    MyDataDocInvoiceId = docInvoice.Id,
                    aa = docInvoice.invoiceHeader.aa,
                    series = docInvoice.invoiceHeader.series,
                    issueDate = docInvoice.invoiceHeader.issueDate,
                    invoiceType = docInvoice.invoiceHeader.invoiceType,
                    vatPaymentSuspension = docInvoice.invoiceHeader.vatPaymentSuspension,
                    currency = docInvoice.invoiceHeader.currency,
                    exchangeRate = docInvoice.invoiceHeader.exchangeRate,
                    correlatedInvoices = docInvoice.invoiceHeader.correlatedInvoices,
                    selfPricing = docInvoice.invoiceHeader.selfPricing, 
                    dispatchDate = docInvoice.invoiceHeader.dispatchDate,
                    dispatchTime = docInvoice.invoiceHeader.dispatchTime,
                    vehicleNumber = docInvoice.invoiceHeader.vehicleNumber,
                    movePurpose = docInvoice.invoiceHeader.movePurpose
                };

                transmittedDoc.paymentMethodDetailType = new List<MyDataPaymentMethodDetailDTO>();
                for (int i = 0; i < docInvoice.paymentMethods.paymentMethodDetails.Count; i++)
                {
                    transmittedDoc.paymentMethodDetailType.Add(new MyDataPaymentMethodDetailDTO
                    {
                        Id = docInvoice.paymentMethods.paymentMethodDetails[i].Id,
                        Created = docInvoice.paymentMethods.paymentMethodDetails[i].Created,
                        Modified = docInvoice.paymentMethods.paymentMethodDetails[i].Modified,
                        MyDataDocInvoiceId = docInvoice.Id,
                        amount = docInvoice.paymentMethods.paymentMethodDetails[i].amount,
                        type = docInvoice.paymentMethods.paymentMethodDetails[i].type,
                        paymentMethodInfo = docInvoice.paymentMethods.paymentMethodDetails[i].paymentMethodInfo
                    });
                }

                transmittedDoc.invoiceDetails = new List<MyDataInvoiceRowTypeDTO>();
                for (int i = 0; i < docInvoice.invoiceDetails.Count; i++)
                {
                    if (docInvoice.invoiceDetails[i].incomeClassification != null)
                    {
                        transmittedDoc.invoiceDetails.Add(new MyDataInvoiceRowTypeDTO
                        {
                            Id = docInvoice.invoiceDetails[i].Id,
                            Created = docInvoice.invoiceDetails[i].Created,
                            Modified = docInvoice.invoiceDetails[i].Modified,
                            MyDataDocInvoiceId = docInvoice.Id,
                            lineNumber = docInvoice.invoiceDetails[i].lineNumber,
                            netValue = docInvoice.invoiceDetails[i].netValue,
                            vatCategory = docInvoice.invoiceDetails[i].vatCategory,
                            vatAmount = docInvoice.invoiceDetails[i].vatAmount,
                            quantity = docInvoice.invoiceDetails[i].quantity,
                            measurementUnit = docInvoice.invoiceDetails[i].measurementUnit,
                            invoiceDetailType = docInvoice.invoiceDetails[i].invoiceDetailType,
                            vatExemptionCategory = docInvoice.invoiceDetails[i].vatExemptionCategory,
                            discountOption = docInvoice.invoiceDetails[i].discountOption,
                            withheldAmount = docInvoice.invoiceDetails[i].withheldAmount,
                            withheldPercentCategory = docInvoice.invoiceDetails[i].withheldPercentCategory,
                            stampDutyAmount = docInvoice.invoiceDetails[i].stampDutyAmount,
                            stampDutyPercentCategory = docInvoice.invoiceDetails[i].stampDutyPercentCategory,
                            feesAmount = docInvoice.invoiceDetails[i].feesAmount,
                            feesPercentCategory = docInvoice.invoiceDetails[i].feesPercentCategory,
                            otherTaxesPercentCategory = docInvoice.invoiceDetails[i].otherTaxesPercentCategory,
                            otherTaxesAmount = docInvoice.invoiceDetails[i].otherTaxesAmount,
                            deductionsAmount = docInvoice.invoiceDetails[i].deductionsAmount,
                            lineComments = docInvoice.invoiceDetails[i].lineComments,
                            incomeClassification = new MyDataIncomeClassificationDTO
                            {
                                Id = docInvoice.invoiceDetails[i].incomeClassification.Id,
                                Created = docInvoice.invoiceDetails[i].incomeClassification.Created,
                                Modified = docInvoice.invoiceDetails[i].incomeClassification.Modified,
                                MyDataInvoiceDetailsId = docInvoice.invoiceDetails[i].Id,
                                amount = docInvoice.invoiceDetails[i].incomeClassification.amount,
                                classificationCategory = docInvoice.invoiceDetails[i].incomeClassification.classificationCategory,
                                classificationType = docInvoice.invoiceDetails[i].incomeClassification.classificationType,
                                optionalId = docInvoice.invoiceDetails[i].incomeClassification.optionalId
                            }
                        });
                    }
                    else
                    {
                        transmittedDoc.invoiceDetails.Add(new MyDataInvoiceRowTypeDTO
                        {
                            Id = docInvoice.invoiceDetails[i].Id,
                            Created = docInvoice.invoiceDetails[i].Created,
                            Modified = docInvoice.invoiceDetails[i].Modified,
                            MyDataDocInvoiceId = docInvoice.Id,
                            lineNumber = docInvoice.invoiceDetails[i].lineNumber,
                            netValue = docInvoice.invoiceDetails[i].netValue,
                            vatCategory = docInvoice.invoiceDetails[i].vatCategory,
                            vatAmount = docInvoice.invoiceDetails[i].vatAmount,
                            quantity = docInvoice.invoiceDetails[i].quantity,
                            measurementUnit = docInvoice.invoiceDetails[i].measurementUnit,
                            invoiceDetailType = docInvoice.invoiceDetails[i].invoiceDetailType,
                            vatExemptionCategory = docInvoice.invoiceDetails[i].vatExemptionCategory,
                            discountOption = docInvoice.invoiceDetails[i].discountOption,
                            withheldAmount = docInvoice.invoiceDetails[i].withheldAmount,
                            withheldPercentCategory = docInvoice.invoiceDetails[i].withheldPercentCategory,
                            stampDutyAmount = docInvoice.invoiceDetails[i].stampDutyAmount,
                            stampDutyPercentCategory = docInvoice.invoiceDetails[i].stampDutyPercentCategory,
                            feesAmount = docInvoice.invoiceDetails[i].feesAmount,
                            feesPercentCategory = docInvoice.invoiceDetails[i].feesPercentCategory,
                            otherTaxesPercentCategory = docInvoice.invoiceDetails[i].otherTaxesPercentCategory,
                            otherTaxesAmount = docInvoice.invoiceDetails[i].otherTaxesAmount,
                            deductionsAmount = docInvoice.invoiceDetails[i].deductionsAmount,
                            lineComments = docInvoice.invoiceDetails[i].lineComments
                        });
                    }
                    
                }
                //transsmittedDoc.invoiceDetails = new MyDataInvoiceRowTypeDTO { Id = docInvoice.invoiceHeader.Id, Created = docInvoice.invoiceHeader.Created, Modified = docInvoice.invoiceHeader.Modified, MyDataDocInvoiceId = docInvoice.Id, };

                transmittedDoc.taxesTotals = new List<MyDataTaxesDTO>();
                for (int i = 0; i < docInvoice.taxesTotals.Count; i++)
                {
                    transmittedDoc.taxesTotals.Add(new MyDataTaxesDTO
                    {
                        Id = docInvoice.taxesTotals[i].taxes.Id,
                        Created = docInvoice.taxesTotals[i].taxes.Created,
                        Modified = docInvoice.taxesTotals[i].taxes.Modified,
                        MyDataDocInvoiceId = docInvoice.Id,
                        taxAmount = docInvoice.taxesTotals[i].taxes.taxAmount,
                        taxCategory = docInvoice.taxesTotals[i].taxes.taxCategory,
                        taxType = docInvoice.taxesTotals[i].taxes.taxType,
                        taxunderlyingValueType = docInvoice.taxesTotals[i].taxes.taxunderlyingValueType
                    });
                }

                transmittedDoc.invoiceSummary = new MyDataInvoiceSummaryDTO {
                    Id = docInvoice.invoiceSummary.Id,
                    Created = docInvoice.invoiceSummary.Created,
                    Modified = docInvoice.invoiceSummary.Modified,
                    MyDataDocInvoiceId = docInvoice.Id,
                    totalNetValue = docInvoice.invoiceSummary.totalNetValue,
                    totalVatAmount = docInvoice.invoiceSummary.totalVatAmount,
                    totalWithheldAmounr = docInvoice.invoiceSummary.totalWithheldAmounr,
                    totalFeesAmount = docInvoice.invoiceSummary.totalFeesAmount,
                    totalStumpDutyAmount = docInvoice.invoiceSummary.totalStumpDutyAmount,
                    totalOtherTaxesAmount = docInvoice.invoiceSummary.totalOtherTaxesAmount,
                    totalDeductionsAmount = docInvoice.invoiceSummary.totalDeductionsAmount,
                    totalGrossValue = docInvoice.invoiceSummary.totalGrossValue
                };


                await _transmittedDocRepo.Insert(transmittedDoc);

                transmittedDocs.Add(transmittedDoc);

            }
            if (requestedDocs.cancelledInvoicesDoc != null)
            {
                foreach (CancelledInvoice cancelledInvoiceDoc in requestedDocs.cancelledInvoicesDoc.cancelledInvoice)
                {
                    MyDataCancelledInvoicesDocDTO cancelledInvoicesDocDTO = new MyDataCancelledInvoicesDocDTO();
                    cancelledInvoicesDocDTO.Id = cancelledInvoiceDoc.Id;
                    cancelledInvoicesDocDTO.Created = cancelledInvoiceDoc.Created;
                    cancelledInvoicesDocDTO.Modified = cancelledInvoiceDoc.Modified;
                    cancelledInvoicesDocDTO.invoiceMark = cancelledInvoiceDoc.invoiceMark;
                    cancelledInvoicesDocDTO.cancellationMark = cancelledInvoiceDoc.cancellationMark;
                    cancelledInvoicesDocDTO.cancellationDate = cancelledInvoiceDoc.cancellationDate;
                }
            }

            return requestedDocs.continuationToken;
            
        }

        public static RequestedDoc DeserializeXml(XmlDocument doc)
        {
            RequestedDoc obj;
            using (TextReader textReader = new StringReader(doc.OuterXml))
            {
                using (XmlTextReader reader = new XmlTextReader(textReader))
                {
                    reader.Namespaces = false;
                    XmlSerializer serializer = new XmlSerializer(typeof(RequestedDoc));
                    obj = (RequestedDoc)serializer.Deserialize(reader);
                }
            }
            return obj;
        }
        
    }
}
