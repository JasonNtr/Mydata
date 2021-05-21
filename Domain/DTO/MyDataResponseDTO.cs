using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataResponseDTO : MyDataEntityDTO
    {
        public virtual Guid MyDataInvoiceId { get; set; }
        public virtual int? index { get; set; }
        public virtual string statusCode { get; set; }
        public virtual string invoiceUid { get; set; }
        public virtual long? invoiceMark { get; set; }
        public virtual string authenticationCode { get; set; }
        public virtual ICollection<MyDataErrorDTO> Errors { get; set; } = new List<MyDataErrorDTO>();
    }
}
