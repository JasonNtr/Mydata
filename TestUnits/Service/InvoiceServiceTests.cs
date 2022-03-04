using Business.ApiServices;
using Business.Services;
using Domain.DTO;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestUnits
{
    class InvoiceServiceTests
    {

        [Test, Order(0)]
        [TestCase(-66666,"UnitTest.xml", 215, 66666, "666666666",  ExpectedResult = true)]
        public async Task<bool> BuildInvoiceBatchProcess(long uid, string fileName, int typeCode, long invoiceNumber, string vat)
        {
            var repo = GetRepo();
            
            var path = Directory.GetParent(AppContext.BaseDirectory).FullName + "\\Configuration\\" + "appsettings.json";
            var appSetting = Create(path);
            var service = new InvoiceService(appSetting, repo, GetMyDataResponseRepo(), GetMyDataCancellationResponseRepo(), GetParticleInformRepo(),GetIMyDataTransmittedDocInvoicesRepo());

            var id = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var date = DateTime.UtcNow;
            MyDataInvoiceDTO myDataInvoiceDTO = new MyDataInvoiceDTO()
            {
                Id = id,
                Created = date,
                Modified = date,
                Uid = uid,
                FileName = fileName,
                StoredXml = "",//AppSettings.Value.folderPath + "/Invoice/Stored/" + filename,
                InvoiceDate = DateTime.UtcNow,
                InvoiceTypeCode = 215,
                InvoiceNumber = invoiceNumber,
                VAT = vat,
                CancellationMark = 66666666
            };

            var result = await service.BuildInvoiceBatchProcess(myDataInvoiceDTO);

            return(result!=null);

        }

        [Test, Order(1)]
        [TestCase(-66666, "UnitTest.xml", 215, -66666, "666666666", ExpectedResult = true)]
        public async Task<bool> CallCancelInvoiceMethod(long uid, string fileName, int typeCode, long invoiceNumber, string vat)
        {
            var repo = GetRepo();

            var path = Directory.GetParent(AppContext.BaseDirectory).FullName + "\\Configuration\\" + "appsettings.json";
            var appSetting = Create(path);
            var service = new InvoiceService(appSetting, repo, GetMyDataResponseRepo(), GetMyDataCancellationResponseRepo(), GetParticleInformRepo(), GetIMyDataTransmittedDocInvoicesRepo());

            var id = Guid.Parse("66666666-6666-6666-6666-666666666666");
            var date = DateTime.UtcNow;
            MyDataInvoiceDTO myDataInvoiceDTO = new MyDataInvoiceDTO()
            {
                Id = id,
                Created = date,
                Modified = date,
                Uid = uid,
                FileName = fileName,
                StoredXml = "",//AppSettings.Value.folderPath + "/Invoice/Stored/" + filename,
                InvoiceDate = DateTime.UtcNow,
                InvoiceTypeCode = 215,
                InvoiceNumber = invoiceNumber,
                VAT = vat,
                CancellationMark = 66666666
            };

            var result = await service.CallCancelInvoiceMethod(myDataInvoiceDTO);

            return (!String.IsNullOrEmpty(result));

        }

        private static InvoiceRepo GetRepo()
        {
            var repo = new InvoiceRepo(GetContext(), GetMapper());
            return repo;
        }

        private static ParticleInform GetParticleInformRepo()
        {
            var repo = new ParticleInform(GetContext(), GetMapper());
            return repo;
        }
        private static MyDataCancellationResponseRepo GetMyDataCancellationResponseRepo()
        {
            var repo = new MyDataCancellationResponseRepo(GetContext(), GetMapper());
            return repo;
        }
        private static MyDataResponseRepo GetMyDataResponseRepo()
        {
            var repo = new MyDataResponseRepo(GetContext(), GetMapper());
            return repo;
        }

        private static MyDataTransmittedDocInvoicesRepo GetIMyDataTransmittedDocInvoicesRepo()
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
