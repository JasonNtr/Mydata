using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataInvoiceSummary : MyDataEntity
    {
        [ForeignKey("MyDataDocInvoiceId")]
        public virtual MyDataTransmittedDocInvoice MyDataDocInvoice { get; set; }
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
