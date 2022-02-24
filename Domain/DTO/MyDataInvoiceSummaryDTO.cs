using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataInvoiceSummaryDTO : MyDataEntityDTO
    {
        public virtual MyDataTransmittedDocInvoiceDTO MyDataDocInvoice { get; set; }
        public virtual Guid MyDataDocInvoiceId { get; set; }
        public virtual double totalNetValue { get; set; }
        public virtual double totalVatAmount { get; set; }
        public virtual double totalWithheldAmounr { get; set; }
        public virtual double totalFeesAmount { get; set; }
        public virtual double totalStumpDutyAmount { get; set; }
        public virtual double totalOtherTaxesAmount { get; set; }
        public virtual double totalDeductionsAmount { get; set; }
        public virtual double totalGrossValue { get; set; }
    }
}
