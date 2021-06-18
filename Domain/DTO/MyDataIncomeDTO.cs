using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.DTO
{
    public class MyDataIncomeDTO : MyDataEntityDTO
    {
        public virtual long? Uid { get; set; }
        public virtual DateTime? IncomeDate { get; set; }
        public virtual string IncomeDateToString => IncomeDate?.ToString("dd/MM/yyyy");
        public virtual long? IncomeNumber { get; set; }
        public virtual string VAT { get; set; }
        public virtual int IncomeTypeCode { get; set; }
        //public virtual MyDataInvoiceTypeDTO InvoiceType { get; set; }
        public virtual string FileName { get; set; }
        public virtual string StoredXml { get; set; }

        public virtual string IncomeMark
        {
            get
            {
                var mark = "";
                mark = MyDataIncomeResponses.FirstOrDefault(x => x.StatusCode.Equals("Success"))?.IncomeMark.ToString();
                return mark;
            }
        }

        public virtual string LastestStatus => MyDataIncomeResponses.Count > 0 ? MyDataIncomeResponses.Last().StatusCode : "Error no status code";
        public virtual ICollection<MyDataIncomeResponseDTO> MyDataIncomeResponses { get; set; } = new List<MyDataIncomeResponseDTO>();
    }
}
