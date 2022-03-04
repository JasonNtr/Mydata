using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Business.ApiServices;
using Business.Services;
using Domain.DTO;
using Infrastructure.Database;
using Infrastructure.Database.RequestDocModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace TestUnits.Repo
{
    public class TransmittedDocInvoicesRepoTests
    {
        [Test, Order(0)]
        public async Task RequestDocs()
        {
            var path = Directory.GetParent(AppContext.BaseDirectory).FullName + "\\Configuration\\" + "appsettings.json";
            var repo = GetRepo();
            var appSetting = Create(path);
            var service = new RequestTransmittedDocsService(appSetting, repo);
            var result = await service.RequestDocs("0");
            Assert.AreEqual(result, 1);
        }

        [Test, Order(1)]
        public async Task RequestTransmittedDocs()
        {
            var path = Directory.GetParent(AppContext.BaseDirectory).FullName + "\\Configuration\\" + "appsettings.json";
            var repo = GetRepo();
            var appSetting = Create(path);
            var service = new RequestTransmittedDocsService(appSetting, repo);
            var result = await service.RequestTransmittedDocs("0");
            Assert.AreEqual(result, 1);
        }

        [Test, Order(2)]
        public void DeserializeXml()
        {
            var path = Directory.GetParent(AppContext.BaseDirectory).FullName + "\\Configuration\\" + "appsettings.json";
            var repo = GetRepo();
            var appSetting = Create(path);
            var service = new RequestTransmittedDocsService(appSetting, repo);

            XmlSerializer mySerializer = new XmlSerializer(typeof(RequestedDoc));
            StreamReader myStreamReader = new StreamReader(@"C:\Users\Aris\Desktop\Desktop TargetFolder\response.xml");
            string readxml = myStreamReader.ReadToEnd();
            var httpResponseContext = readxml;

            var doc = new XmlDocument();
            doc.LoadXml(httpResponseContext);
            var result = service.DeserializeXml(doc);

            Assert.IsNotNull(result.invoicesDoc);
        }

        [Test, Order(3)]
        public async Task CallRequestDocsMethod()
        {
            var path = Directory.GetParent(AppContext.BaseDirectory).FullName + "\\Configuration\\" + "appsettings.json";
            var repo = GetRepo();
            var appSetting = Create(path);
            var service = new RequestTransmittedDocsService(appSetting, repo);
            ContinuationToken continuationToken = null;
            var result = await service.CallRequestDocsMethod("0", continuationToken);

            Assert.IsNotNull(result);
        }

        [Test, Order(4)]
        public async Task CallRequestTransmittedDocsMethod()
        {
            var path = Directory.GetParent(AppContext.BaseDirectory).FullName + "\\Configuration\\" + "appsettings.json";
            var repo = GetRepo();
            var appSetting = Create(path);
            var service = new RequestTransmittedDocsService(appSetting, repo);
            ContinuationToken continuationToken = null;
            var result = await service.CallRequestTransmittedDocsMethod("0", continuationToken);

            Assert.IsNotNull(result);
        }


        private static MyDataTransmittedDocInvoicesRepo GetRepo()
        {
            var repo = new MyDataTransmittedDocInvoicesRepo(GetContext(), GetMapper());
            return repo;
        }

        public static IOptions<AppSettings> Create(string filePath)
        {
            var appSetting = new AppSettings()
            {
                url = "https://mydata-dev.azure-api.net/",
                aade_user_id = "peppas_sa",
                Ocp_Apim_Subscription_Key = "07351d24bdb94be18520269d2314c192",
                folderPath = "C:\\Users\\Aris\\source\\VS19_repos\\1 - Working\\Mydata\\MyDataRepository",
                Auto = false,
            };
            var appSettings = Options.Create(appSetting);
            return appSettings;
        }



        private static ApplicationDbContext GetContext()
        {
            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName + "\\Configuration")
              .AddJsonFile("AppSettings.json", false)
              .Build();
            var connectionstring = $"{configuration["ConnectionStrings:Default"]}";
            var options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(connectionstring)
                    .Options;
            var appcontext = new ApplicationDbContext(options);
            return appcontext;
        }

        private static AutoMapper.IMapper GetMapper()
        {
            var mapperConfig = new AutoMapper.MapperConfiguration(mc =>
            {
                mc.AddProfile(new Business.Mappings.MappingProfiles());
            });
            AutoMapper.IMapper mapper = new AutoMapper.Mapper(mapperConfig);
            return mapper;
        }
    }
}
