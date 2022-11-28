using Domain.AADE;
using Domain.DTO;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Business.ApiServices
{
    public class ExpenseService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly string _connectionString;

        public ExpenseService(IOptions<AppSettings> appSettings, string connectionString)
        {
            _appSettings = appSettings;
            _connectionString = connectionString;
        }

        public async Task GetExpenses(DateTime dateFrom, DateTime dateTo)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            var aadeUserId = _appSettings.Value.aade_user_id;
            var ocpApimSubscriptionKey = _appSettings.Value.Ocp_Apim_Subscription_Key;

            client.DefaultRequestHeaders.Add("aade-user-id", aadeUserId);
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", ocpApimSubscriptionKey);

            // Request parameters
            queryString["dateFrom"] = dateFrom.ToString();
            queryString["dateTo"] = dateTo.ToString();
            queryString["entityVatNumber"] = "{string}";
            queryString["counterVatNumber"] = "{string}";
            queryString["invType"] = "{string}";
            queryString["maxMark"] = "{string}";
            var uri = "https://mydata-dev.azure-api.net/RequestDocs?mark=1";// + queryString;

            var response = await client.GetAsync(uri);

            var responseMessage = await response.Content.ReadAsStringAsync();

            var list = ParseExpensesResponseResult(responseMessage);
        }

        private List<InvoicesDoc> ParseExpensesResponseResult(string httpresponsecontext)
        {
            var invoices = new List<InvoicesDoc>();
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(httpresponsecontext);

                var mySerializer = new XmlSerializer(typeof(List<InvoicesDoc>));

                var myResponseData = new List<InvoicesDoc>();
                using (TextReader reader = new StringReader(xmlDoc.InnerXml))
                {
                    myResponseData = (List<InvoicesDoc>)mySerializer.Deserialize(reader);
                }

                //foreach (var response in myResponseData.response)
                //{
                //    var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfiles()); });
                //    var mapper = new AutoMapper.Mapper(mapperConfig);
                //    var mydataresponse = mapper.Map<MyDataResponseDTO>(response);
                //    mydataresponses.Add(mydataresponse);
                //}

                return myResponseData;
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
                //mydataresponses.Add(mydataresponse);
                return null;
            }
        }
    }
}