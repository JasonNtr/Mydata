using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class MyDataPaymentMethodDetail : MyDataEntity
    {
        
        public virtual MyDataTransmittedDocInvoice? MyDataDocInvoice { get; set; }
        [ForeignKey("MyDataDocInvoiceId")]
        public virtual Guid MyDataDocInvoiceId { get; set; }
        public virtual int type { get; set; }
        public virtual double amount { get; set; }
        public virtual string paymentMethodInfo { get; set; }

    }
}
