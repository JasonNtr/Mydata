using AutoMapper;
using Domain.DTO;
using Domain.Model;
using Infrastructure.Database;
using Infrastructure.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class MyDataTransmittedDocInvoicesRepo : IMyDataTransmittedDocInvoicesRepo
    {
        private readonly ApplicationDbContext ctx;
        private readonly IMapper mapper;

        public MyDataTransmittedDocInvoicesRepo(ApplicationDbContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        public async Task<int> Insert(MyDataTransmittedDocInvoiceDTO transmittedDocDTO)
        {
            var transmittedDoc = new MyDataTransmittedDocInvoice();
            transmittedDoc =  mapper.Map(transmittedDocDTO, transmittedDoc);
            await ctx.MyDataTransmittedDocInvoices.AddAsync(transmittedDoc);
            var result = await ctx.SaveChangesAsync();
            return result;
        }
    }
}
