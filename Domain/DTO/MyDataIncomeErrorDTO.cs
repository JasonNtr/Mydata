using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataIncomeErrorDTO : MyDataEntityDTO
    {
        public virtual Guid MyDataIncomeResponseId { get; set; }
        public virtual MyDataIncomeResponseDTO MyDataIncomeResponseDTO { get; set; }
        public virtual string Message { get; set; }
        public virtual int Code { get; set; }
    }
}
