using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class MyDataCancelInvoice
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual Guid Id { get; set; }
        public virtual long? Uid { get; set; }
        public virtual long? invoiceMark { get; set; }
        public virtual bool invoiceProcessed { get; set; }

        public virtual ICollection<MyDataCancelationResponse> MyDataCancelationResponses { get; set; } = new List<MyDataCancelationResponse>();
    }
}
