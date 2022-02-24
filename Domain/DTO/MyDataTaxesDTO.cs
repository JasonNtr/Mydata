using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataTaxesDTO : MyDataEntityDTO
    {
        public virtual MyDataTransmittedDocInvoiceDTO? MyDataDocInvoice { get; set; }
        public virtual Guid? MyDataDocInvoiceId { get; set; }
        public virtual int? taxType { get; set; }
        public virtual int? taxCategory { get; set; }
        public virtual double? taxunderlyingValueType { get; set; }
        public virtual double? taxAmount { get; set; }
    }
}
