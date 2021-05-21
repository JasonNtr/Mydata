using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Model;

namespace Domain.DTO
{
    public class MyDataInvoiceDTO : MyDataEntityDTO
    {
        public virtual long? Uid { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual string InvoiceDateToString
        {
            get
            {
                return InvoiceDate?.ToString("dd/MM/yyyy");
            }
        }
        public virtual long? InvoiceNumber { get; set; }
        public virtual string VAT { get; set; }
        public virtual int InvoiceTypeCode { get; set; }
        public virtual MyDataInvoiceTypeDTO InvoiceType { get; set; }
        public virtual string FileName { get; set; }
        public virtual string StoredXml { get; set; }
        public virtual string LastestStatus
        {
            get
            {
                if(CancellationMark == null)
                    return MyDataResponses.Count > 0 ? MyDataResponses.Last().statusCode : "Error no status code";
                else
                    return MyDataCancelationResponses.Count > 0 ? MyDataCancelationResponses.Last().statusCode : "Error no status code";
            }
        }
        public virtual string invoiceMark
        {
            get
            {
                var mark = "";
                if (CancellationMark != null)
                {
                    mark = CancellationMark.ToString();
                }
                else if (MyDataResponses.Any(x => x.statusCode.Equals("Success")))
                {
                    mark = MyDataResponses.FirstOrDefault(x => x.statusCode.Equals("Success")).invoiceMark.ToString();
                }
                return mark;
            }
        }
        public virtual ICollection<MyDataResponseDTO> MyDataResponses { get; set; } = new List<MyDataResponseDTO>();


        public virtual long? CancellationMark { get; set; }
        public virtual ICollection<MyDataCancelationResponseDTO> MyDataCancelationResponses { get; set; } = new List<MyDataCancelationResponseDTO>();

    }
}
