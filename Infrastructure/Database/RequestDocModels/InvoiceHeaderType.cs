using Domain.DTO;
using System;

namespace Infrastructure.Database.RequestDocModels
{
    public class InvoiceHeaderType : MyDataEntityDTO
    {
        public virtual string series { get; set; }
        public virtual string aa { get; set; }
        public virtual DateTime issueDate { get; set; }
        public virtual string invoiceType { get; set; }
        public virtual bool? vatPaymentSuspension { get; set; }
        public virtual string currency { get; set; }
        //Not used
        public virtual double? exchangeRate { get; set; }
        public virtual long? correlatedInvoices { get; set; }
        public virtual bool? selfPricing { get; set; }
        public virtual DateTime? dispatchDate { get; set; }
        public virtual string dispatchTime { get; set; }
        public virtual string vehicleNumber { get; set; }
        public virtual int? movePurpose { get; set; }


    }
}
