using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Services;
using Domain.DTO;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;

namespace TestUnits.Repo
{
    public class IncomeRepoTests
    {

        [Test, Order(0)]
        public async Task Insert_Successful()
        {

            var repo = GetRepo();
            var testIncomeDTO = new MyDataIncomeDTO()
            {
                Created = DateTime.Now,
                Uid = 15111995,
                FileName = "Test Data File",
                Id = System.Guid.Parse("13131313-1313-1313-1313-131313131313"),
                IncomeDate = DateTime.Now,
                IncomeNumber = 15111995,
                Modified = DateTime.Now,
                IncomeTypeCode = 0,
                VAT = "TEST VAT",
                StoredXml = "Test Data Stored XML",
                MyDataIncomeResponses = new List<MyDataIncomeResponseDTO>()
            };
            var myDataIncomeDTO = await repo.Insert(testIncomeDTO);
            var result = myDataIncomeDTO != null;
            Assert.AreEqual(result, true);
        }

        [Test, Order(1)]
        public async Task Update_Successful()
        {
            var repo = GetRepo();
            var myDataIncomeDTO = await repo.GetByUid(15111995);
            myDataIncomeDTO.VAT = "Updated Test VAT";
            myDataIncomeDTO.Modified = DateTime.Now;

            var updatedDataIncomeDTO = await repo.Update(myDataIncomeDTO);
            var myDataIncomeDTOSerialized = JsonConvert.SerializeObject(myDataIncomeDTO);
            var updatedDataIncomeDTOSerialized = JsonConvert.SerializeObject(updatedDataIncomeDTO);

            Assert.AreEqual(myDataIncomeDTOSerialized, updatedDataIncomeDTOSerialized);
        }


        private static IncomeRepo GetRepo()
        {
            IncomeRepo repo = new IncomeRepo(GetDatabase(), GetMapper());
            return repo;
        }

        private static ApplicationDbContext GetDatabase()
        {
            var configuration = GetConfig();
            string connectionstring = $"{configuration["ConnectionStrings:Default"]}";
            DbContextOptions<ApplicationDbContext> options =
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer(connectionstring)
                    .Options;
            ApplicationDbContext appcontext = new ApplicationDbContext(options);
            return appcontext;
        }

        private static IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
            return builder.Build();
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
