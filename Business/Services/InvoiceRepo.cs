using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Model;
using Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class InvoiceRepo : Repo, IInvoiceRepo 
    {
         
        public InvoiceRepo(string connectionString) : base(connectionString)
        {

        }
        

        public async Task<int> AddIfNotExist(MyDataInvoiceDTO mydatainvoicedto)
        {
            var context = GetContext();
            var mydatainvoice = await context.MyDataInvoices
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
                    Mapper.Map(mydataresponsedto, mydataresponse);
                    context.Attach(mydataresponse);
                    mydatainvoice.MyDataResponses.Add(mydataresponse);
                }

                context.Update(mydatainvoice);
                result = await context.SaveChangesAsync();
            }
            else
            {
                var newmydatainvoice = new MyDataInvoice();
                Mapper.Map(mydatainvoicedto, newmydatainvoice);
                await context.MyDataInvoices.AddAsync(newmydatainvoice);
                result = await context.SaveChangesAsync();
            }
            return result;
        }
        
        public async Task<MyDataInvoiceDTO> Get()
        {
            var context = GetContext();
            var mydatainvoice = 
                await context.MyDataInvoices
                    .Where(x => x.MyDataResponses.Any(y => y.statusCode.Equals("Success")))
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.InvoiceType)
                    .FirstOrDefaultAsync();
            
            var mydatainvoicedto = Mapper.Map<MyDataInvoiceDTO>(mydatainvoice);
            return mydatainvoicedto;
        }

        public async Task<List<MyDataInvoiceDTO>> GetInvoicesWithSuccessStatusCodeFor2021()
        {
            var context = GetContext();
            var mydatainvoice =
                await context.MyDataInvoices
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.InvoiceType)
                    .Where(x => x.MyDataResponses.Any(y => y.statusCode.Equals("Success") && x.InvoiceTypeCode != 215
                    && (x.InvoiceDate >= new DateTime(2021, 1, 1, 0, 0, 0) && x.InvoiceDate <= new DateTime(2022, 1, 1, 0, 0, 0))))
                    .ToListAsync();
            

            var mydatainvoicedto = Mapper.Map<List<MyDataInvoiceDTO>>(mydatainvoice);
            return mydatainvoicedto;
        }

        public async Task<bool> ExistedUid(long? Uid)
        {
            var context = GetContext();
            if (Uid == null) return false;

            var exist =
                await context.MyDataInvoices
                    .AnyAsync(x => x.Uid == Uid);
            return exist;           
        }

        public async Task<List<MyDataInvoiceDTO>> GetList(DateTime selectedDate)
        {
            var context = GetContext();
            var list =
                await context.MyDataInvoices
                    .Where(x => x.Modified.Year == selectedDate.Year && x.Modified.Month == selectedDate.Month && x.Modified.Day == selectedDate.Day)
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.InvoiceType)
                    .ToListAsync();

            var mydatainvoicedto = Mapper.Map<List<MyDataInvoiceDTO>>(list);
            return mydatainvoicedto;
        }

        public async Task<List<MyDataInvoiceDTO>> GetList()
        {
            var context = GetContext();
            var list =
                await context.MyDataInvoices
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.InvoiceType)
                    .OrderByDescending(x => x.Modified)
                    .Take(20)
                    .ToListAsync();

            var mydatainvoicedto = Mapper.Map<List<MyDataInvoiceDTO>>(list);
            return mydatainvoicedto;
        }

        public async Task<MyDataInvoiceDTO> GetByUid(long? Uid)
        {
            var context = GetContext();
            var mydatainvoice =
                await context.MyDataInvoices
                    .FirstOrDefaultAsync(x => x.Uid == Uid);
            var mydatainvoicedto = Mapper.Map<MyDataInvoiceDTO>(mydatainvoice);
            return mydatainvoicedto;
        }

        public async Task<int> AddResponses(MyDataInvoiceDTO mydatainvoicedto)
        {
            var context = GetContext();
            var newmydatainvoice = await context.MyDataInvoices
                .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                .Include(p => p.MyDataCancelationResponses).ThenInclude(p => p.Errors)
                .Include(p => p.InvoiceType)
                .FirstOrDefaultAsync(x => x.Uid == mydatainvoicedto.Uid);
            Mapper.Map(mydatainvoicedto, newmydatainvoice);
            context.Update(newmydatainvoice);
            var result = await context.SaveChangesAsync();
            return result;
        }

        public async Task<MyDataInvoiceDTO> Insert(MyDataInvoiceDTO myDataInvoiceDTO)
        {
            var context = GetContext();
            var newMyDataInvoice = new MyDataInvoice();
            Mapper.Map(myDataInvoiceDTO, newMyDataInvoice);
            await context.MyDataInvoices.AddAsync(newMyDataInvoice);
            var result = await context.SaveChangesAsync();
            var myDataInvoiceDTORefreshed = await GetByUid(newMyDataInvoice.Uid);
            return myDataInvoiceDTORefreshed;
        }

        public async Task<MyDataInvoiceDTO> Update(MyDataInvoiceDTO myDataInvoiceDTO)
        {
            var context = GetContext();
            var newMyDataInvoice = await context.MyDataInvoices
                .FirstOrDefaultAsync(x => x.Uid == myDataInvoiceDTO.Uid);

            if (newMyDataInvoice == null) return null;
            Mapper.Map(myDataInvoiceDTO, newMyDataInvoice);
            context.Update(newMyDataInvoice);
            var result = await context.SaveChangesAsync();
            var myDataInvoiceDTORefreshed = await GetByUid(newMyDataInvoice.Uid);
            return myDataInvoiceDTORefreshed;
        }

        public async Task<List<MyDataInvoiceDTO>> GetList(DateTime fromDate, DateTime toDate)
        {
            var context = GetContext();
            var newFromDate = fromDate.Date;
            var newToDate = toDate.AddDays(1).Date;
            var list =
                await context.MyDataInvoices
                    .Where(x => x.Modified.Date >= newFromDate && x.Modified.Date <= newToDate)
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.MyDataCancelationResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.InvoiceType)
                    .OrderByDescending(x => x.Modified)
                    .ToListAsync();

            var mydatainvoicedto = Mapper.Map<List<MyDataInvoiceDTO>>(list);
            return mydatainvoicedto;
           
        }

        public async Task<MyDataInvoiceDTO> GetByMark(long invoiceMark)
        {
            var context = GetContext();
            var mydatainvoice =
                await context.MyDataInvoices
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.MyDataCancelationResponses).ThenInclude(p => p.Errors)
                    .FirstOrDefaultAsync(x => x.MyDataResponses.Any(x => x.invoiceMark == invoiceMark));
            var mydatainvoicedto = Mapper.Map<MyDataInvoiceDTO>(mydatainvoice);
            return mydatainvoicedto;
        }
        public long GetMaxUid()
        {
            var context = GetContext();
            var invoiceMaxUid = (long)context.MyDataInvoices.Max(x => x.Uid);
            return invoiceMaxUid;
        }

        public async Task<bool> InsertOrUpdateRangeForPost(List<MyDataInvoiceDTO> transferModelMyDataInvoices)
        {
            var context = GetContext();

            var invoices = new List<MyDataInvoice>();
            Mapper.Map(transferModelMyDataInvoices, invoices);

            foreach (var invoice in invoices)
            {
                var exists = await context.MyDataInvoices.AnyAsync(x =>
                    x.InvoiceDate == invoice.InvoiceDate && x.Uid == invoice.Uid &&
                    x.InvoiceNumber == invoice.InvoiceNumber && x.VAT.Equals(invoice.VAT));

                if (exists)
                {
                    var myInvoice = await context.MyDataInvoices.FirstOrDefaultAsync(x =>
                        x.InvoiceDate == invoice.InvoiceDate && x.Uid == invoice.Uid &&
                        x.InvoiceNumber == invoice.InvoiceNumber && x.VAT.Equals(invoice.VAT));
                    foreach (var response in invoice.MyDataResponses)
                    {
                        response.MyDataInvoiceId = myInvoice.Id;
                    }

                    await context.AddRangeAsync(invoice.MyDataResponses);
                }
                else
                {
                    await context.AddAsync(invoice);
                }
            }
            
            
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> InsertOrUpdateRangeForCancel(List<MyDataInvoiceDTO> transferModelMyDataInvoices)
        {
            var context = GetContext();

            var invoices = new List<MyDataCancelInvoice>();
            Mapper.Map(transferModelMyDataInvoices, invoices);

            foreach (var invoice in invoices)
            {
                var exists = await context.MyDataCancelInvoices.AnyAsync(x => x.Uid == invoice.Uid);

                if (exists)
                {
                    var myInvoice = await context.MyDataInvoices.FirstOrDefaultAsync(x => x.Uid == invoice.Uid);
                    foreach (var response in invoice.MyDataCancelationResponses)
                    {
                        response.MyDataInvoiceId = myInvoice.Id;
                    }

                    await context.AddRangeAsync(invoice.MyDataCancelationResponses);
                }
                else
                {
                    await context.AddAsync(invoice);
                }
            }


            var result = await context.SaveChangesAsync();
            return result > 0;
        }
    }
}
