using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.AADE
{
    public class InvoiceResponse
    {
       
        public virtual int? index { get; set; }
        public virtual string statusCode { get; set; }
        public virtual string invoiceUid { get; set; }
        public virtual long? invoiceMark { get; set; }
        
        //[XmlElement]
        //public virtual List<MyDataErrorDTO> Errors { get; set; } = new List<MyDataErrorDTO>();
    }
}
