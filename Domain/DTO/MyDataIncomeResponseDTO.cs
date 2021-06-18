using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataIncomeResponseDTO : MyDataEntityDTO
    {
        public virtual Guid MyDataIncomeId { get; set; }
        public virtual int? Index { get; set; }
        public virtual string StatusCode { get; set; }
        public virtual string IncomeUid { get; set; }
        public virtual long? IncomeMark { get; set; }
        public virtual string AuthenticationCode { get; set; }
        public virtual ICollection<MyDataIncomeErrorDTO> Errors { get; set; } = new List<MyDataIncomeErrorDTO>();
    }
}
