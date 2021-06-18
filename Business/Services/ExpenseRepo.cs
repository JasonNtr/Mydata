using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Infrastructure.Database;
using Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ExpenseRepo : IExpenseRepo
    {
        private readonly ApplicationDbContext ctx;
        private readonly IMapper mapper;

        public ExpenseRepo(ApplicationDbContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }
        public Task<int> AddIfNotExist(MyDataInvoiceDTO mydatainvoicedto)
        {
            throw new NotImplementedException();
        }

        public Task<MyDataInvoiceDTO> Insert(MyDataInvoiceDTO mydatainvoicedto)
        {
            throw new NotImplementedException();
        }

        public Task<MyDataInvoiceDTO> Update(MyDataInvoiceDTO mydatainvoicedto)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddResponses(MyDataInvoiceDTO mydatainvoicedto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistedUid(long? Uid)
        {
            throw new NotImplementedException();
        }

        public Task<MyDataInvoiceDTO> Get()
        {
            throw new NotImplementedException();
        }

        public Task<MyDataInvoiceDTO> GetByUid(long? Uid)
        {
            throw new NotImplementedException();
        }

        public Task<List<MyDataInvoiceDTO>> GetList()
        {
            throw new NotImplementedException();
        }

        public async Task<List<MyDataInvoiceDTO>> GetList(DateTime fromDate, DateTime ToDate)
        {
            try
            {
                var list =
                    await ctx.MyDataInvoices
                        .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                        .Include(p => p.InvoiceType)
                        .OrderByDescending(x => x.Modified)
                        .Take(20)
                        .ToListAsync();

                var mydatainvoicedto = mapper.Map<List<MyDataInvoiceDTO>>(list);
                return mydatainvoicedto;
            }
            catch (Exception ex)
            {

            }
            return new List<MyDataInvoiceDTO>();
        }

        public Task<List<MyDataInvoiceDTO>> GetList(DateTime selectedDate)
        {
            throw new NotImplementedException();
        }

        public Task<MyDataInvoiceDTO> GetByMark(long invoiceMark)
        {
            throw new NotImplementedException();
        }
    }
}
