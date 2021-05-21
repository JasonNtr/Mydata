using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataCancelationErrorDTO : MyDataEntityDTO
    {
        public virtual Guid MyDataCancelationResponseId { get; set; }
        public virtual MyDataCancelationResponseDTO MyDataCancelationResponse { get; set; }
        public virtual string Message { get; set; }
        public virtual int Code { get; set; }
    }
}
