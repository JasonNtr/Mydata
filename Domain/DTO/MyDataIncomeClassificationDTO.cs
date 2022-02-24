using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataIncomeClassificationDTO : MyDataEntityDTO
    {
        public virtual MyDataInvoiceRowTypeDTO? MyDataInvoiceDocRowType { get; set; }
        public virtual Guid? MyDataInvoiceDetailsId { get; set; }
        public int? optionalId { get; set; }
        public virtual string classificationType { get; set; }
        public virtual string classificationCategory { get; set; }
        public virtual double? amount { get; set; }
    }
}
