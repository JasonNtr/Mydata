using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataInvoiceDetails : MyDataEntity
    {
        [ForeignKey("MyDataDocInvoiceId")]
        public virtual MyDataTransmittedDocInvoice MyDataDocInvoice { get; set; }
        public virtual Guid MyDataDocInvoiceId { get; set; }
        public virtual int lineNumber { get; set; }
        public virtual double netValue { get; set; }
        public virtual string vatCategory { get; set; }
        public virtual double vatAmount { get; set; }
    }
}
