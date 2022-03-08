using AutoMapper;
using Domain.DTO;
using Domain.Model;
using Infrastructure.Database;
using Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
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

        public async Task<int> Update(MyDataTransmittedDocInvoiceDTO transmittedDocDTO)
        {
            var transmittedDocInvoice = await ctx.MyDataTransmittedDocInvoices
                .Include(r=>r.issuer).Include(r=>r.counterpart).Include(r=>r.invoiceHeaderType).Include(r => r.paymentMethodDetailType).Include(r => r.invoiceDetails).Include(r => r.taxesTotals).Include(r => r.invoiceSummary)
                .FirstOrDefaultAsync(x => x.mark == transmittedDocDTO.mark);
            mapper.Map(transmittedDocDTO, transmittedDocInvoice);
            ctx.Update(transmittedDocInvoice);
            var result = await ctx.SaveChangesAsync();
            //var myDataInvoiceDTORefreshed = await GetByUid(transmittedDocInvoice.Uid);
            return result;
        }

        public async Task<bool> ExistsMark(long? mark)
        {
            if (mark == null) { return false; }

            var exist = await ctx.MyDataTransmittedDocInvoices.AnyAsync(x => x.mark == mark);
            return exist;
        }

        public async Task<MyDataTransmittedDocInvoiceDTO> GetByMark(long? mark)
        {
            if (mark == null) { return null; }

            var docInvoice = await ctx.MyDataTransmittedDocInvoices.Include(r => r.issuer).Include(r => r.counterpart).Include(r => r.invoiceHeaderType).Include(r => r.paymentMethodDetailType).Include(r => r.invoiceDetails).Include(r => r.taxesTotals).Include(r => r.invoiceSummary)
                .FirstOrDefaultAsync(x => x.mark == mark);

            var docInvoiceDTO = mapper.Map<MyDataTransmittedDocInvoiceDTO>(docInvoice);
            return docInvoiceDTO;
        }

    }
}
