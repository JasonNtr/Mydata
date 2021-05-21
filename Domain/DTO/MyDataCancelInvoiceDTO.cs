using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataCancelInvoiceDTO
    {
        public virtual Guid Id { get; set; }
        public virtual long? Uid { get; set; }
        public virtual long? invoiceMark { get; set; }
        public virtual bool invoiceProcessed { get; set; }
    }
}
