using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Helpers;
using Domain.DTO;
using Domain.Model;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Business.Services
{
    public class InvoiceRepo : IInvoiceRepo
    {
        private readonly ApplicationDbContext ctx;
        private readonly IMapper mapper;
        public InvoiceRepo(ApplicationDbContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        public async Task<int> AddIfNotExist(MyDataInvoiceDTO mydatainvoicedto)
        {
            var mydatainvoice = await ctx.MyDataInvoices
                .Where(x => x.Uid.Equals(mydatainvoicedto.Uid))
                .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                .Include(p => p.InvoiceType)
                .FirstOrDefaultAsync();
            var result = 0;
            if (mydatainvoice != null)
            {
                mydatainvoice.InvoiceDate = mydatainvoicedto.InvoiceDate;
                mydatainvoice.InvoiceTypeCode = mydatainvoicedto.InvoiceTypeCode;
                mydatainvoice.FileName = mydatainvoicedto.FileName;
                mydatainvoice.StoredXml = mydatainvoicedto.StoredXml;
                mydatainvoice.InvoiceNumber = mydatainvoicedto.InvoiceNumber;
                mydatainvoice.Modified = DateTime.UtcNow;

                if (mydatainvoicedto.MyDataResponses.Count > 0)
                {
                    var mydataresponsedto = mydatainvoicedto.MyDataResponses.FirstOrDefault();
                    mydataresponsedto.MyDataInvoiceId = mydatainvoice.Id;

                    var mydataresponse = new MyDataResponse();
                    mapper.Map(mydataresponsedto, mydataresponse);
                    ctx.Attach(mydataresponse);
                    mydatainvoice.MyDataResponses.Add(mydataresponse);
                }

                ctx.Update(mydatainvoice);
                result = await ctx.SaveChangesAsync();
            }
            else
            {
                var newmydatainvoice = new MyDataInvoice();
                mapper.Map(mydatainvoicedto, newmydatainvoice);
                await ctx.MyDataInvoices.AddAsync(newmydatainvoice);
                result = await ctx.SaveChangesAsync();
            }
            return result;
        }
        
        public async Task<MyDataInvoiceDTO> Get()
        {
            var mydatainvoice = 
                await ctx.MyDataInvoices
                    .Where(x => x.MyDataResponses.Any(y => y.statusCode.Equals("Success")))
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.InvoiceType)
                    .FirstOrDefaultAsync();
            
            var mydatainvoicedto = mapper.Map<MyDataInvoiceDTO>(mydatainvoice);
            return mydatainvoicedto;
        }

        public async Task<bool> ExistedUid(long? Uid)
        {
            if (Uid == null) return false;

            var exist =
                await ctx.MyDataInvoices
                    .AnyAsync(x => x.Uid == Uid);
            return exist;
        }

        public async Task<List<MyDataInvoiceDTO>> GetList(DateTime selectedDate)
        {
            try
            {
                var list =
                    await ctx.MyDataInvoices
                        .Where(x => x.Modified.Year == selectedDate.Year && x.Modified.Month == selectedDate.Month && x.Modified.Day == selectedDate.Day)
                        .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                        .Include(p => p.InvoiceType)
                        .ToListAsync();

                var mydatainvoicedto = mapper.Map<List<MyDataInvoiceDTO>>(list);
                return mydatainvoicedto;
            }
            catch (Exception ex)
            {

            }
            return new List<MyDataInvoiceDTO>();
        }

        public async Task<List<MyDataInvoiceDTO>> GetList()
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

        public async Task<MyDataInvoiceDTO> GetByUid(long? Uid)
        {
            var mydatainvoice =
                await ctx.MyDataInvoices
                    .FirstOrDefaultAsync(x => x.Uid == Uid);
            var mydatainvoicedto = mapper.Map<MyDataInvoiceDTO>(mydatainvoice);
            return mydatainvoicedto;
        }

        public async Task<int> AddResponses(MyDataInvoiceDTO mydatainvoicedto)
        {
            var newmydatainvoice = await ctx.MyDataInvoices
                .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                .Include(p => p.MyDataCancelationResponses).ThenInclude(p => p.Errors)
                .Include(p => p.InvoiceType)
                .FirstOrDefaultAsync(x => x.Uid == mydatainvoicedto.Uid);
            try
            {
                mapper.Map(mydatainvoicedto, newmydatainvoice);
                ctx.Update(newmydatainvoice);
                var result = await ctx.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                var error = ex.ToString();
                Debug.WriteLine(error);
                return 0;
            }
        }

        public async Task<MyDataInvoiceDTO> Insert(MyDataInvoiceDTO mydatainvoicedto)
        {
            var newmydatainvoice = new MyDataInvoice();
            mapper.Map(mydatainvoicedto, newmydatainvoice);
            await ctx.MyDataInvoices.AddAsync(newmydatainvoice);
            var result = await ctx.SaveChangesAsync();
            var mydatainvoicedtorefreshed = await GetByUid(newmydatainvoice.Uid);
            return mydatainvoicedtorefreshed;
        }

        public async Task<MyDataInvoiceDTO> Update(MyDataInvoiceDTO mydatainvoicedto)
        {
            var newmydatainvoice = await ctx.MyDataInvoices
                .FirstOrDefaultAsync(x => x.Uid == mydatainvoicedto.Uid);
            mapper.Map(mydatainvoicedto, newmydatainvoice);
            ctx.Update(newmydatainvoice);
            var result = await ctx.SaveChangesAsync();
            var mydatainvoicedtorefreshed = await GetByUid(newmydatainvoice.Uid);
            return mydatainvoicedtorefreshed;
        }

        public async Task<List<MyDataInvoiceDTO>> GetList(DateTime fromDate, DateTime ToDate)
        {
           
            try
            {
                var list =
                    await ctx.MyDataInvoices
                        .Where(x => x.Modified.Date >= fromDate.Date && x.Modified.Date <= ToDate.Date)
                        .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                        .Include(p => p.MyDataCancelationResponses).ThenInclude(p => p.Errors)
                        .Include(p => p.InvoiceType)
                        .OrderByDescending(x => x.Modified)
                        .ToListAsync();

                var mydatainvoicedto = mapper.Map<List<MyDataInvoiceDTO>>(list);
                return mydatainvoicedto;
            }
            catch (Exception ex)
            {

            }
            return new List<MyDataInvoiceDTO>();
        }

        public async Task<MyDataInvoiceDTO> GetByMark(long invoiceMark)
        {
            var mydatainvoice =
                await ctx.MyDataInvoices
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.MyDataCancelationResponses).ThenInclude(p => p.Errors)
                    .FirstOrDefaultAsync(x => x.MyDataResponses.Any(x => x.invoiceMark == invoiceMark));
            var mydatainvoicedto = mapper.Map<MyDataInvoiceDTO>(mydatainvoice);
            return mydatainvoicedto;
        }
    }
}
