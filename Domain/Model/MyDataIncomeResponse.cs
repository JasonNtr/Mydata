using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataIncomeResponse : MyDataEntity
    {
        [ForeignKey("MyDataInvoiceId")]
        public virtual Guid MyDataIncomeId { get; set; }
        public virtual MyDataIncome MyDataIncome { get; set; }
        public virtual int? Index { get; set; }
        public virtual string StatusCode { get; set; }
        public virtual string IncomeUid { get; set; }
        public virtual long? IncomeMark { get; set; }
        public virtual string AuthenticationCode { get; set; }
        public virtual ICollection<MyDataIncomeError> Errors { get; set; } = new List<MyDataIncomeError>();
    }
}
