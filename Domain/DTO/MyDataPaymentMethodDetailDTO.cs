using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataPaymentMethodDetailDTO : MyDataEntityDTO
    {
        public virtual MyDataTransmittedDocInvoiceDTO? MyDataDocInvoice { get; set; }
        public virtual Guid MyDataDocInvoiceId { get; set; }
        public virtual int type { get; set; }
        public virtual double amount { get; set; }
        public virtual string paymentMethodInfo { get; set; }
    }
}
