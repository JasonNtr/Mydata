using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataInvoiceDetailsDTO : MyDataEntityDTO
    {
        public virtual MyDataTransmittedDocInvoiceDTO MyDataDocInvoice { get; set; }
        public virtual Guid MyDataDocInvoiceId { get; set; }
        public virtual int lineNumber { get; set; }
        public virtual double netValue { get; set; }
        public virtual string vatCategory { get; set; }
        public virtual double vatAmount { get; set; }
    }
}
