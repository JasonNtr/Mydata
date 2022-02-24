using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataTaxes : MyDataEntity
    {
        public virtual MyDataTransmittedDocInvoice? MyDataDocInvoice { get; set; }
        [ForeignKey("MyDataDocInvoiceId")]
        public virtual Guid? MyDataDocInvoiceId { get; set; }
        public virtual int? taxType { get; set; }
        public virtual int? taxCategory { get; set; }
        public virtual double? taxunderlyingValueType { get; set; }
        public virtual double? taxAmount { get; set; }
    }
}
