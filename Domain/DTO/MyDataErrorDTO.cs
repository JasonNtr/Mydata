using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataErrorDTO : MyDataEntityDTO
    {
        public virtual Guid MyDataResponseId { get; set; }
        public virtual MyDataResponseDTO MyDataResponse { get; set; }
        public virtual string message { get; set; }
        public virtual int code { get; set; }
    }
}
