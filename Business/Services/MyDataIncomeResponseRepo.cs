using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Domain.Model;
using Infrastructure.Database;
using Infrastructure.Interfaces.Services;

namespace Business.Services
{
    public class MyDataIncomeResponseRepo : IMyDataIncomeResponseRepo
    {
        private readonly ApplicationDbContext ctx;
        private readonly IMapper mapper;

        public MyDataIncomeResponseRepo(ApplicationDbContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        public async Task<int> Insert(MyDataIncomeResponseDTO myDataIncomeResponseDTO)
        {
            var myDataResponse = new MyDataIncomeResponse();
            mapper.Map(myDataIncomeResponseDTO, myDataResponse);
            await ctx.MyDataIncomeResponses.AddAsync(myDataResponse);
            var result = await ctx.SaveChangesAsync();
            return result;
        }
    }
}