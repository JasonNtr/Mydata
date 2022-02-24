using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class MyDataExpensesClassification : MyDataEntity
    {
        public virtual MyDataTransmittedDocInvoice? MyDataDocInvoice { get; set; }
        [ForeignKey("MyDataDocInvoiceId")]
        public virtual Guid? MyDataDocInvoiceId { get; set; }
        public int? optionalId { get; set; }
        public virtual string classificationType { get; set; }
        public virtual string classificationCategory { get; set; }
        public virtual double? amount { get; set; }
    }
}
