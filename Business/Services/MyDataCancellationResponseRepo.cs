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
    public class MyDataCancellationResponseRepo : IMyDataCancellationResponseRepo
    {
        private readonly ApplicationDbContext ctx;
        private readonly IMapper mapper;

        public MyDataCancellationResponseRepo(ApplicationDbContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        public async Task<int> Insert(MyDataCancelationResponseDTO mydatacancellationresponsedto)
        {
            var mydatacancellationresponse = new MyDataCancelationResponse();
            mapper.Map(mydatacancellationresponsedto, mydatacancellationresponse);
            await ctx.MyDataCancelationResponses.AddAsync(mydatacancellationresponse);
            var result = await ctx.SaveChangesAsync();
            return result;
        }
    }
}
