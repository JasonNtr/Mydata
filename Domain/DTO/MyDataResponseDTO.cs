using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Domain.DTO
{
    public class MyDataResponseDTO : MyDataEntityDTO
    {
        public virtual Guid MyDataInvoiceId { get; set; }
        public virtual int? index { get; set; }
        public virtual string statusCode { get; set; }
        public virtual string invoiceUid { get; set; }
        public virtual long? invoiceMark { get; set; }
        public virtual long? cancellationMark { get; set; }
        public virtual string qrUrl { get; set; }

        public virtual string authenticationCode { get; set; }
        [XmlElement]
        public virtual List<MyDataErrorDTO> errors { get; set; } = new List<MyDataErrorDTO>();
    }
}
