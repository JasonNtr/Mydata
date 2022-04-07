using Domain.DTO;
using Infrastructure.Database.RequestDocModels;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Business.ApiServices
{
    public class RequestTransmittedDocsService : IRequestTransmittedDocsService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IMyDataTransmittedDocInvoicesRepo _transmittedDocRepo;

        public RequestTransmittedDocsService(IOptions<AppSettings> appSettings, IMyDataTransmittedDocInvoicesRepo myDataTransmittedDocInvoicesRepo)
        {
            _appSettings = appSettings;
            _transmittedDocRepo = myDataTransmittedDocInvoicesRepo;
        }
        public async Task<ContinuationToken> ConvertRequestedDocsToDTO(RequestedDoc requestedDocs)
        {
            int cnt = 1;
            var transmittedDocs = new List<MyDataTransmittedDocInvoiceDTO>();
            if (requestedDocs.invoicesDoc != null)
            {
                foreach (var docInvoice in requestedDocs.invoicesDoc.invoice)
                {
                    Debug.WriteLine(cnt);
                    cnt++;
                    var exists = await _transmittedDocRepo.ExistsMark(docInvoice.mark);
                    MyDataTransmittedDocInvoiceDTO transmittedDoc = null;
                    if (exists)
                    {
                        Debug.WriteLine("Invoice with mark : " + docInvoice.mark + " already exists");
                        //transmittedDoc = await _transmittedDocRepo.GetByMark(docInvoice.mark);
                        continue;
                    }
                    else
                    {
                        transmittedDoc = new MyDataTransmittedDocInvoiceDTO();
                        transmittedDoc.Id = docInvoice.Id;
                        transmittedDoc.Created = docInvoice.Created;
                        transmittedDoc.Modified = docInvoice.Modified;
                        transmittedDoc.Uid = docInvoice.Uid;
                        transmittedDoc.mark = docInvoice.mark;
                        transmittedDoc.cancelledByMark = docInvoice.cancelledByMark;
                        transmittedDoc.authenticationCode = docInvoice.authenticationCode;
                    }

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

                    transmittedDoc.invoiceHeaderType = new MyDataInvoiceHeaderTypeDTO
                    {
                        Id = docInvoice.invoiceHeader.Id,
                        Created = docInvoice.invoiceHeader.Created,
                        Modified = docInvoice.invoiceHeader.Modified,
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

                    transmittedDoc.invoiceSummary = new MyDataInvoiceSummaryDTO
                    {
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

                    var insertResult = await _transmittedDocRepo.Insert(transmittedDoc);
                    if (insertResult > 0)
                    {
                        Debug.WriteLine("Inserted invoice : " + transmittedDoc.Id + "   with mark : " + transmittedDoc.mark);
                    }
                    transmittedDocs.Add(transmittedDoc);

                }
            }
            

            if (requestedDocs.cancelledInvoicesDoc != null)
            {
                cnt = 1;
                foreach (var cancelledInvoiceDoc in requestedDocs.cancelledInvoicesDoc.cancelledInvoice)
                {
                    var cancelledInvoicesDocDTO = new MyDataCancelledInvoicesDocDTO();
                    cancelledInvoicesDocDTO.Id = cancelledInvoiceDoc.Id;
                    cancelledInvoicesDocDTO.Created = cancelledInvoiceDoc.Created;
                    cancelledInvoicesDocDTO.Modified = cancelledInvoiceDoc.Modified;
                    cancelledInvoicesDocDTO.invoiceMark = cancelledInvoiceDoc.invoiceMark;
                    cancelledInvoicesDocDTO.cancellationMark = cancelledInvoiceDoc.cancellationMark;
                    cancelledInvoicesDocDTO.cancellationDate = cancelledInvoiceDoc.cancellationDate;
                    Debug.WriteLine(cnt + ") Cancellled Invoice : " + cancelledInvoiceDoc.cancellationMark);
                    cnt++;
                }
            }

            return requestedDocs.continuationToken;
        }

        public async Task<int> RequestDocsFirstImplementation(string mark)
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

                var doc = new XmlDocument();
                doc.LoadXml(httpResponseContext);
                var invoices = DeserializeXml(doc);

                //this is to pass the values of the requestedDoc to the DTO in order for them to be saved
                continuationToken = await ConvertRequestedDocsToDTO(invoices);


                //var doc1 = new XmlDocument();
                //doc1.LoadXml(httpResponseContext);
                //if (httpResponseContext.TrimStart().StartsWith("<?"))
                //{
                //    doc1.RemoveChild(doc1.FirstChild);
                //}
                //var json = JsonConvert.SerializeXmlNode(doc1, Formatting.None, true);
                //var docInvoice = JsonConvert.DeserializeObject<RequestDocs>(json);

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

        public async Task<int> RequestTransmittedDocs(string mark)
        {
            ContinuationToken continuationToken = null;
            do
            {
                var httpResponseContext = await CallRequestTransmittedDocsMethod(mark, continuationToken);
                httpResponseContext = httpResponseContext.Replace("icls:", "");

                var doc = new XmlDocument();
                doc.LoadXml(httpResponseContext);
                var invoices = DeserializeXml(doc);
                continuationToken = await ConvertRequestedDocsToDTO(invoices);
                //this is to pass the values of the requestedDoc to the DTO in order for them to be saved
                

            } while (continuationToken != null);
            return 1;
        }
        public async Task<int> RequestDocs(string mark)
        {
            ContinuationToken continuationToken = null;
            do
            {
                var httpResponseContext = await CallRequestDocsMethod(mark, continuationToken);
                httpResponseContext = httpResponseContext.Replace("icls:", "");

                var doc = new XmlDocument();
                doc.LoadXml(httpResponseContext);
                var invoices = DeserializeXml(doc);

                //this is to pass the values of the requestedDoc to the DTO in order for them to be saved
                continuationToken = await ConvertRequestedDocsToDTO(invoices);

            } while (continuationToken != null);
            return 1;
        }
        public async Task<string> CallRequestDocsMethod(string mark, ContinuationToken continuationToken)
        {
            var url = _appSettings.Value.url;
            var aadeUserId = _appSettings.Value.aade_user_id;
            var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;
            var client = new HttpClient();

            var result = "";
            // Request headers
            client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
            var nextPartitionKey = (continuationToken == null ? "" : continuationToken.nextPartitionKey);
            var nextRowKey = (continuationToken == null ? "" : continuationToken.nextRowKey);
            var uri = url + "/RequestDocs?mark=" + mark + "&nextPartitionKey=" + nextPartitionKey + "&nextRowKey=" + nextRowKey;
            //var uri = url + "/RequestDocs?mark=" + 0; // + queryString;
            //var uri = url + "/RequestTransmittedDocs?mark=" + 0; // + queryString;
            var byteData = new byte[0];
            using var content = new ByteArrayContent(byteData);
            //content.Headers.Add("mark", myDataInvoiceDTO.CancellationMark.ToString());

            var httpResponse = await client.GetAsync(uri);
            if (!httpResponse.IsSuccessStatusCode)
                return ""+result;

            result = await httpResponse.Content.ReadAsStringAsync();

            return result;
        }

        public async Task<string> CallRequestTransmittedDocsMethod(string mark, ContinuationToken continuationToken)
        {
            var url = _appSettings.Value.url;
            var aadeUserId = _appSettings.Value.aade_user_id;
            var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;
            var client = new HttpClient();

            var result = "";
            // Request headers
            client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);
            var nextPartitionKey = (continuationToken == null ? "" : continuationToken.nextPartitionKey);
            var nextRowKey = (continuationToken == null ? "" : continuationToken.nextRowKey);
            var uri = url + "/RequestTransmittedDocs?mark=" + mark + "&nextPartitionKey=" + nextPartitionKey + "&nextRowKey=" + nextRowKey;
            var byteData = new byte[0];
            using var content = new ByteArrayContent(byteData);

            var httpResponse = await client.GetAsync(uri);
            if (!httpResponse.IsSuccessStatusCode)
                return "" + result;

            result = await httpResponse.Content.ReadAsStringAsync();
            return result;
        }

        public RequestedDoc DeserializeXml(XmlDocument doc)
        {
            RequestedDoc obj;
            using (var textReader = new StringReader(doc.OuterXml))
            {
                using (var reader = new XmlTextReader(textReader))
                {
                    reader.Namespaces = false;
                    var serializer = new XmlSerializer(typeof(RequestedDoc));
                    obj = (RequestedDoc)serializer.Deserialize(reader);
                }
            }
            return obj;
        }

    }
}
