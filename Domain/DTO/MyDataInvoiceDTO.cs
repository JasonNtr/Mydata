using System;
using System.Collections.Generic;
using System.Linq;

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
        public virtual long? CancellationMark { get; set; }
        public virtual string VAT { get; set; }
        public virtual int InvoiceTypeCode { get; set; }
        public virtual MyDataInvoiceTypeDTO InvoiceType { get; set; }
        public virtual string FileName { get; set; }
        public virtual string StoredXml { get; set; }
        public virtual string LastestStatus
        {
            get
            {
                if(MyDataResponses.Any(x => x.statusCode.Equals("Success"))){
                    return "Success";
                }
                if (MyDataCancellationResponses.Count > 0)
                {
                    var statusCode = MyDataCancellationResponses.OrderBy(x => x.Created).Last().statusCode;
                    return statusCode;
                }
                else
                {
                    return MyDataResponses.Count > 0 ? MyDataResponses.OrderBy(x => x.Created).Last().statusCode : "Error no status code";
                }
               
            }
        }
        public virtual string invoiceMark
        {
            get
            {
                var mark = "";
                if (MyDataResponses.Any(x => x.statusCode.Equals("Success")))
                {
                    mark = MyDataResponses.FirstOrDefault(x => x.statusCode.Equals("Success")).invoiceMark.ToString();
                }
                return mark;
            }
        }
        public virtual ICollection<MyDataResponseDTO> MyDataResponses { get; set; } = new List<MyDataResponseDTO>();
        public virtual ICollection<MyDataCancelationResponseDTO> MyDataCancellationResponses { get; set; } = new List<MyDataCancelationResponseDTO>();

        public virtual string FilePath { get; set; }

        
        public ParticleDTO Particle { get; set; }

    }
}
