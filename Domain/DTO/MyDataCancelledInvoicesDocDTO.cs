using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataCancelledInvoicesDocDTO : MyDataEntityDTO
    {
        public virtual long? invoiceMark { get; set; }
        public virtual long? cancellationMark { get; set; }
        public virtual DateTime? cancellationDate { get; set; }
    }
}
