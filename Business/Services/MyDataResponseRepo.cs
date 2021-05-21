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
    public class MyDataResponseRepo :  IMyDataResponseRepo
    {
        private readonly ApplicationDbContext ctx;
        private readonly IMapper mapper;

        public MyDataResponseRepo(ApplicationDbContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        public async Task<int> Insert(MyDataResponseDTO mydataresponsedto)
        {
            var mydataresponse = new MyDataResponse();
            mapper.Map(mydataresponsedto, mydataresponse);
            await ctx.MyDataResponses.AddAsync(mydataresponse);
            var result = await ctx.SaveChangesAsync();
            return result;
        }
    }
}
