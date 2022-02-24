using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataInvoiceHeaderTypeDTO : MyDataEntityDTO
    {
        public virtual MyDataTransmittedDocInvoiceDTO MyDataDocInvoice { get; set; }
        public virtual Guid MyDataDocInvoiceId { get; set; }
        public virtual string series { get; set; }
        public virtual string aa { get; set; }
        public virtual DateTime issueDate { get; set; }
        public virtual string invoiceType { get; set; }
        public virtual bool? vatPaymentSuspension { get; set; }
        public virtual string currency { get; set; }


        //Not Used
        public virtual double? exchangeRate { get; set; }
        public virtual long? correlatedInvoices { get; set; }
        public virtual bool? selfPricing { get; set; }
        public virtual DateTime? dispatchDate { get; set; }
        public virtual string dispatchTime { get; set; }
        public virtual string vehicleNumber { get; set; }
        public virtual int? movePurpose { get; set; }
    }
}
