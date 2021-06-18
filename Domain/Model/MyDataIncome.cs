using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class MyDataIncome : MyDataEntity
    {
        public virtual long? Uid { get; set; }
        public virtual DateTime? IncomeDate { get; set; }
        public virtual long? IncomeNumber { get; set; }
        public virtual string VAT { get; set; }
        public virtual int IncomeTypeCode { get; set; }
        //public virtual MyDataInvoiceType InvoiceType { get; set; }
        public virtual string FileName { get; set; }
        public virtual string StoredXml { get; set; }
        public virtual ICollection<MyDataIncomeResponse> MyDataIncomeResponses { get; set; } = new List<MyDataIncomeResponse>();
        public virtual long? CancellationMark { get; set; }
        //public virtual ICollection<MyDataCancelationResponse> MyDataCancelationResponses { get; set; } = new List<MyDataCancelationResponse>();
    }
}
