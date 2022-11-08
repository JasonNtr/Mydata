using Domain.Model;
using System;

namespace Domain.DTO
{
    public class MyDataCancelationErrorDTO : MyDataEntityDTO
    {
        public virtual Guid MyDataCancelationResponseId { get; set; }
        public virtual MyDataCancelationResponseDTO MyDataCancelationResponse { get; set; }
      
        public virtual string message { get; set; }
        public virtual int code { get; set; }
    }
}