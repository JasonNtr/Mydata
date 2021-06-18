using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataExpenseResponseDTO : MyDataEntity
    {
        [ForeignKey("MyDataInvoiceId")]
        public virtual Guid MyDataExpenseId { get; set; }
        public virtual MyDataExpenseDTO MyDataExpense { get; set; }
        public virtual int? index { get; set; }
        public virtual string statusCode { get; set; }
        public virtual string ExpenseUid { get; set; }
        public virtual long? ExpenseMark { get; set; }
        public virtual string authenticationCode { get; set; }
        public virtual ICollection<MyDataError> Errors { get; set; } = new List<MyDataError>();
    }
}
