using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Infrastructure.Database;
using Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Business.Services
{
    public class MyDataCancelInvoiceRepo : IMyDataCancelInvoiceRepo
    {
        private readonly IMapper mapper;
        private readonly IOptions<ConnectionStrings> ConnectionStrings;



        public MyDataCancelInvoiceRepo(IMapper mapper, IOptions<ConnectionStrings> ConnectionStrings)
        {
            this.mapper = mapper;
            this.ConnectionStrings = ConnectionStrings;
          
        }

        public async Task<MyDataCancelInvoiceDTO> GetNext()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConnectionStrings.Value.Default)
                .Options;
            await using var ctx = new ApplicationDbContext(contextOptions);
            var mydatacancelinvoice =
                await ctx.MyDataCancelInvoices.Where(x => !x.invoiceProcessed).FirstOrDefaultAsync();
            var mydatacancelinvoicedtodto = mapper.Map<MyDataCancelInvoiceDTO>(mydatacancelinvoice);
            return mydatacancelinvoicedtodto;
        }

        public async Task<bool> Update(Guid Id)
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(ConnectionStrings.Value.Default)
                .Options;
            await using var ctx = new ApplicationDbContext(contextOptions);
            var mydatacancelinvoice =
                await ctx.MyDataCancelInvoices.Where(x => x.Id.Equals(Id)).FirstOrDefaultAsync();
            mydatacancelinvoice.invoiceProcessed = true;
            var result = await ctx.SaveChangesAsync();
            return result>1;
        }
    }
}
