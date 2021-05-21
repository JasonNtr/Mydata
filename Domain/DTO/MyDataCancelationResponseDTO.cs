using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataCancelationResponseDTO : MyDataEntityDTO
    {
        public virtual Guid MyDataInvoiceId { get; set; }
        public virtual MyDataInvoiceDTO MyDataInvoice { get; set; }
        public virtual long? cancellationMark { get; set; }
        public virtual string statusCode { get; set; }
        public virtual ICollection<MyDataCancelationErrorDTO> Errors { get; set; } = new List<MyDataCancelationErrorDTO>();
    }
}
