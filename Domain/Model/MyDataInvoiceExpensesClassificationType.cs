using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class MyDataInvoiceExpensesClassificationType : MyDataEntity
    {
        public virtual long invoiceMark { get; set; }
        public virtual long? classificationMark { get; set; }
        public virtual int? transactionMode { get; set; }
        public virtual int? lineNumber { get; set; }
        public virtual MyDataExpensesClassification? expensesClassificationDetailData { get; set; }
    }
}
