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

        public async Task<List<MyDataInvoiceDTO>> GetInvoicesWithSuccessStatusCode()
        {
            var mydatainvoice =
                await ctx.MyDataInvoices
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.InvoiceType)
                    .Where(x => x.MyDataResponses.Any(y => y.statusCode.Equals("Success") && x.InvoiceTypeCode != 215 
                    && (x.InvoiceDate >= new DateTime(2021, 1, 1, 0, 0, 0) && x.InvoiceDate <= new DateTime(2022, 1, 1, 0, 0, 0))))// && x.InvoiceNumber == 105356))                    
                    .ToListAsync();
            

            var mydatainvoicedto = mapper.Map<List<MyDataInvoiceDTO>>(mydatainvoice);
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

        public async Task<MyDataInvoiceDTO> Insert(MyDataInvoiceDTO myDataInvoiceDTO)
        {
            var newMyDataInvoice = new MyDataInvoice();
            mapper.Map(myDataInvoiceDTO, newMyDataInvoice);
            await ctx.MyDataInvoices.AddAsync(newMyDataInvoice);
            var result = await ctx.SaveChangesAsync();
            var myDataInvoiceDTORefreshed = await GetByUid(newMyDataInvoice.Uid);
            return myDataInvoiceDTORefreshed;
        }

        public async Task<MyDataInvoiceDTO> Update(MyDataInvoiceDTO myDataInvoiceDTO)
        {
            var newMyDataInvoice = await ctx.MyDataInvoices
                .FirstOrDefaultAsync(x => x.Uid == myDataInvoiceDTO.Uid);
            mapper.Map(myDataInvoiceDTO, newMyDataInvoice);
            ctx.Update(newMyDataInvoice);
            var result = await ctx.SaveChangesAsync();
            var myDataInvoiceDTORefreshed = await GetByUid(newMyDataInvoice.Uid);
            return myDataInvoiceDTORefreshed;
        }

        public async Task<List<MyDataInvoiceDTO>> GetList(DateTime fromDate, DateTime toDate)
        {
           
            try
            {
                var list =
                    await ctx.MyDataInvoices
                        .Where(x => x.Modified.Date >= fromDate.Date && x.Modified.Date <= toDate.Date)
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
        public long GetMaxUid()
        {
            var invoiceMaxUid = (long)ctx.MyDataInvoices.Max(x => x.Uid);
            return invoiceMaxUid;
        }
    }
}
