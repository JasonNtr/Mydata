using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataExpensesClassificationDTO : MyDataEntityDTO
    {
        public virtual MyDataTransmittedDocInvoiceDTO? MyDataDocInvoice { get; set; }
        public virtual Guid? MyDataDocInvoiceId { get; set; }
        public int? optionalId { get; set; }
        public virtual string classificationType { get; set; }
        public virtual string classificationCategory { get; set; }
        public virtual double? amount { get; set; }
    }
}
