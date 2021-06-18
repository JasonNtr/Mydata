using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataIncomeError : MyDataEntity
    {
        [ForeignKey("MyDataIncomeResponseId")]
        public virtual Guid MyDataIncomeResponseId { get; set; }
        public virtual MyDataIncomeResponse MyIncomeDataResponse { get; set; }
        public virtual string Message { get; set; }
        public virtual int Code { get; set; }
    }
}
