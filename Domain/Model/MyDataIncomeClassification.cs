using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataIncomeClassification : MyDataEntity
    {
        public virtual MyDataInvoiceRowType? MyDataInvoiceDocRowType { get; set; }
        [ForeignKey("MyDataInvoiceDocRowTypeId")]
        public virtual Guid? MyDataInvoiceDetailsId { get; set; }
        public int? optionalId { get; set; }
        [StringLength(50)]
        public virtual string classificationType { get; set; }
        [StringLength(50)]
        public virtual string classificationCategory { get; set; }
        public virtual double? amount { get; set; } 
    }
}
