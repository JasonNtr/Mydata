using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataInvoiceRowType : MyDataEntity
    {
        public virtual MyDataTransmittedDocInvoice? MyDataDocInvoice { get; set; }
        [ForeignKey("MyDataDocInvoiceId")]
        public virtual Guid? MyDataDocInvoiceId { get; set; }
        public virtual int lineNumber { get; set; }
        public virtual double netValue { get; set; }
        [StringLength(20)]
        public virtual string vatCategory { get; set; }
        public virtual double vatAmount { get; set; }



        //
        public virtual double? quantity { get; set; }
        public virtual int? measurementUnit { get; set; }
        public virtual int? invoiceDetailType { get; set; }
        public virtual int? vatExemptionCategory { get; set; }
        //public virtual ShipType? dienergia { get; set; }
        public virtual bool? discountOption { get; set; }
        public virtual double? withheldAmount { get; set; }
        public virtual int? withheldPercentCategory { get; set; }
        public virtual double? stampDutyAmount { get; set; }
        public virtual int? stampDutyPercentCategory { get; set; }
        public virtual double? feesAmount { get; set; }
        public virtual int? feesPercentCategory { get; set; }
        public virtual int? otherTaxesPercentCategory { get; set; }
        public virtual double? otherTaxesAmount { get; set; }
        public virtual double? deductionsAmount { get; set; }
        [StringLength(150)]
        public virtual string lineComments { get; set; }
        public virtual MyDataIncomeClassification? incomeClassification { get; set; }
    }
}
