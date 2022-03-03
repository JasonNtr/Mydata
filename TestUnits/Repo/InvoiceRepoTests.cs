using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Business.Services;
using Domain.DTO;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;


namespace TestUnits.Repo
{
    public class InvoiceRepoTests
    {

        [Test, Order(0)]
        public async Task GetInvoicesWithSuccessStatusCode()
        {

            var repo = GetRepo();
            var myDataSuccessfulInvoices = await repo.GetInvoicesWithSuccessStatusCodeFor2021();
            var result = (myDataSuccessfulInvoices != null) && (myDataSuccessfulInvoices.Count > 1);
            Assert.AreEqual(result, true);
        }

        private static InvoiceRepo GetRepo()
        {
            var repo = new InvoiceRepo(GetContext(), GetMapper());
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
