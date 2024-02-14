using Domain.AADE;
using Domain.DTO;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class InvoiceRepo : Repo
    {
        public InvoiceRepo(string connectionString) : base(connectionString)
        {
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

        public async Task<List<MyDataInvoiceDTO>> GetList(DateTime fromDate, DateTime toDate)
        {
            var context = GetContext();
            var newFromDate = fromDate.Date;
            var newToDate = toDate.AddDays(1).Date;
             
            var list =
                await context.MyDataInvoices
                    .AsNoTracking()
                    .Where(x =>  x.InvoiceDate.Date >= newFromDate && x.InvoiceDate.Date <= newToDate)
                    .Include(p => p.MyDataResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.MyDataCancellationResponses).ThenInclude(p => p.Errors)
                    .Include(p => p.InvoiceType)
                    .OrderByDescending(x => x.Modified)
                    .ToListAsync();


            var mydatainvoicedto = Mapper.Map<List<MyDataInvoiceDTO>>(list);
            return mydatainvoicedto;
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
                    x.InvoiceNumber == invoice.InvoiceNumber );

                if (exists)
                {
                    var myInvoice = await context.MyDataInvoices.FirstOrDefaultAsync(x =>
                        x.InvoiceDate == invoice.InvoiceDate && x.Uid == invoice.Uid &&
                        x.InvoiceNumber == invoice.InvoiceNumber );

                    
                     
                    foreach (var response in invoice.MyDataResponses)
                    {
                        response.MyDataInvoiceId = myInvoice.Id;
                    }

                    if (myInvoice.VAT != invoice.VAT || myInvoice.InvoiceTypeCode != invoice.InvoiceTypeCode)
                    {
                        myInvoice.VAT = invoice.VAT;
                        myInvoice.InvoiceTypeCode = invoice.InvoiceTypeCode;
                        context.MyDataInvoices.Update(myInvoice);
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

        public async Task<bool> InsertOrUpdateRangeForCancel(List<MyDataCancelInvoiceDTO> transferModelMyDataInvoices)
        {
            var context = GetContext();

            var invoices = new List<MyDataCancelInvoice>();
            Mapper.Map(transferModelMyDataInvoices, invoices);

            foreach (var invoice in invoices)
            {
                var exists = await context.MyDataCancelInvoices.AnyAsync(x => x.Uid == invoice.Uid);

                if (exists)
                {
                    var myInvoice = await context.MyDataCancelInvoices.FirstOrDefaultAsync(x => x.Uid == invoice.Uid);
                    myInvoice.invoiceMark = invoice.invoiceMark;
                    myInvoice.invoiceProcessed = invoice.invoiceProcessed;
                    context.Update(myInvoice);
                }
                else
                {
                    await context.AddAsync(invoice);
                }
            }

            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateCancellationMark(MyDataInvoiceDTO myDataInvoice, string particleMark)
        {
            var context = GetContext();

            var myInvoice = await context.MyDataInvoices.FirstOrDefaultAsync(x =>
                x.InvoiceDate == myDataInvoice.InvoiceDate && x.Uid == myDataInvoice.Uid &&
                x.InvoiceNumber == myDataInvoice.InvoiceNumber && x.VAT.Equals(myDataInvoice.VAT));
            myInvoice.CancellationMark = long.Parse(particleMark);
            context.Update(myInvoice);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> InsertCancelResponses(List<MyDataInvoiceDTO> invoiceList)
        {
            var context = GetContext();
            var invoices = new List<MyDataInvoice>();
            Mapper.Map(invoiceList, invoices);

            foreach (var invoice in invoices)
            {
                var myInvoice = await context.MyDataInvoices.FirstOrDefaultAsync(x =>
                    x.InvoiceDate == invoice.InvoiceDate && x.Uid == invoice.Uid &&
                    x.InvoiceNumber == invoice.InvoiceNumber && x.VAT.Equals(invoice.VAT));

                foreach (var response in invoice.MyDataCancellationResponses)
                {
                    response.MyDataInvoiceId = myInvoice.Id;
                }

                await context.AddRangeAsync(invoice.MyDataCancellationResponses);
            }

            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> InsertResponse(MyDataResponseDTO responseDTO)
        {
            var context = GetContext();
            var response = Mapper.Map<MyDataResponse>(responseDTO);
            context.MyDataResponses.Add(response);
            var result = await context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<MyDataInvoiceTypeDTO> GetEmptyInvoiceType()
        {
            var context = GetContext();
            var invoiceType = await context.InvoiceTypes.FirstOrDefaultAsync(x=>x.Code == 666);
            var invoiceTypeDTO = Mapper.Map<MyDataInvoiceTypeDTO>(invoiceType);
            return invoiceTypeDTO;
        }


    }
}