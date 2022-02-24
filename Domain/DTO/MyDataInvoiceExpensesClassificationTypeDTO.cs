using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataInvoiceExpensesClassificationTypeDTO : MyDataEntityDTO
    {
        public virtual long invoiceMark { get; set; }
        public virtual long? classificationMark { get; set; }
        public virtual int? transactionMode { get; set; }
        public virtual int? lineNumber { get; set; }
        public virtual MyDataExpensesClassificationDTO? expensesClassificationDetailData { get; set; }
    }
}
