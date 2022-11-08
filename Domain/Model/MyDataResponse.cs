using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class MyDataResponse : MyDataEntity
    {
        [ForeignKey("MyDataInvoiceId")]
        public virtual Guid MyDataInvoiceId { get; set; }

        public virtual MyDataInvoice MyDataInvoice { get; set; }
        public virtual int? index { get; set; }
        public virtual string statusCode { get; set; }
        public virtual string invoiceUid { get; set; }
        public virtual long? invoiceMark { get; set; }
        public virtual string authenticationCode { get; set; }
        public virtual ICollection<MyDataError> Errors { get; set; } = new List<MyDataError>();
    }
}