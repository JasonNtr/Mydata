using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataInvoiceRowTypeDTO : MyDataEntityDTO
    {
        public virtual MyDataTransmittedDocInvoiceDTO? MyDataDocInvoice { get; set; }
        public virtual Guid? MyDataDocInvoiceId { get; set; }
        public virtual int lineNumber { get; set; }
        public virtual double netValue { get; set; }
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
        public virtual string lineComments { get; set; }
        public virtual MyDataIncomeClassificationDTO? incomeClassification { get; set; }
    }
}
