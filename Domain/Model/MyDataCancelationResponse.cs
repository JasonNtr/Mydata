using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataCancelationResponse : MyDataEntity
    {
        [ForeignKey("MyDataInvoiceId")]
        public virtual Guid MyDataInvoiceId { get; set; }
        public virtual MyDataInvoice MyDataInvoice { get; set; }
        public virtual long? cancellationMark { get; set; }
        public virtual string statusCode { get; set; }
        public virtual ICollection<MyDataCancelationError> Errors { get; set; } = new List<MyDataCancelationError>();
    }
}
