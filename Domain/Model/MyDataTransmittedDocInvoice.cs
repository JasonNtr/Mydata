using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Model
{
    public class MyDataTransmittedDocInvoice : MyDataEntity
    {
        [StringLength(40)]
        public virtual string? Uid { get; set; }
        [StringLength(100)]
        public virtual string? authenticationCode { get; set; }
        public virtual long? mark { get; set; }
        public virtual long? cancelledByMark { get; set; }
        public virtual ICollection<MyDataPartyType> issuer { get; set; } = new List<MyDataPartyType>();
        public virtual ICollection<MyDataPartyType> counterpart { get; set; } = new List<MyDataPartyType>();
        public virtual MyDataInvoiceHeaderType invoiceHeaderType { get; set; }
        public virtual ICollection<MyDataPaymentMethodDetail> paymentMethodDetailType { get; set; } = new List<MyDataPaymentMethodDetail>();
        public virtual ICollection<MyDataInvoiceRowType> invoiceDetails { get; set; } = new List<MyDataInvoiceRowType>();
        public virtual ICollection<MyDataTaxes> taxesTotals { get; set; } = new List<MyDataTaxes>();
        public virtual MyDataInvoiceSummary invoiceSummary { get; set; }
    }
}
