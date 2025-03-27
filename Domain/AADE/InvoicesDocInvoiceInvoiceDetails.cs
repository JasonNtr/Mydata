using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoiceInvoiceDetails
    {
        private uint lineNumberField;

        private byte recTypeField;
        private string itemCodeField;


        private string itemDescrField;

        private byte fuelCodeField;

        private decimal quantityField;

        private int measurementUnitField;


        private byte invoiceDetailTypeField;

        private decimal netValueField;

        private int vatCategoryField;

        private decimal vatAmountField;

        private byte? vatExemptionCategoryField;

        private InvoicesDocInvoiceInvoiceDetailsDienergia dienergiaField;

        private bool discountOptionField;

        private decimal withheldAmountField;

        private byte withheldPercentCategoryField;

        private decimal stampDutyAmountField;

        private int? stampDutyPercentCategoryField;

        private decimal feesAmountField;

        private byte feesPercentCategoryField;

        private byte otherTaxesPercentCategoryField;

        private decimal otherTaxesAmountField;

        private decimal deductionsAmountField;

        private string lineCommentsField;

        private InvoicesDocInvoiceInvoiceDetailsIncomeClassification[] incomeClassificationField;

        private InvoicesDocInvoiceInvoiceDetailsExpensesClassification[] expensesClassificationField;

        private decimal quantity15Field;
        private int otherMeasurementUnitQuantityField;

        private string otherMeasurementUnitTitleField;
        /// <remarks/>
        public uint lineNumber
        {
            get
            {
                return this.lineNumberField;
            }
            set
            {
                this.lineNumberField = value;
            }
        }

        [XmlIgnore]
        public byte recType
        {
            get
            {
                return this.recTypeField;
            }
            set
            {
                this.recTypeField = value;
            }
        }

        public string itemCode
        {
            get
            {
                return this.itemCodeField;
            }
            set
            {
                this.itemCodeField = value;
            }
        }

        public string itemDescr
        {
            get
            {
                return this.itemDescrField;
            }
            set
            {
                this.itemDescrField = value;
            }
        }

        [XmlIgnore]
        public byte fuelCode
        {
            get
            {
                return this.fuelCodeField;
            }
            set
            {
                this.fuelCodeField = value;
            }
        }

        public string quantity
        {
            get
            {
                return this.quantityField.ToString("G", System.Globalization.CultureInfo.InvariantCulture); 
            }
            set
            {
                this.quantityField = Convert.ToDecimal(value, System.Globalization.CultureInfo.InvariantCulture); 
            }
        }
        public bool ShouldSerializequantity()
        {
            return quantityField > 0;
        }

        public int measurementUnit
        {
            get
            {
                return this.measurementUnitField;
            }
            set
            {
                this.measurementUnitField = value;
            }
        }

        public bool ShouldSerializemeasurementUnit()
        {
            return measurementUnitField > 0;
        }


        [XmlIgnore]
        public byte invoiceDetailType
        {
            get
            {
                return this.invoiceDetailTypeField;
            }
            set
            {
                this.invoiceDetailTypeField = value;
            }
        }

        /// <remarks/>
        public decimal netValue
        {
            get
            {
                return this.netValueField;
            }
            set
            {
                this.netValueField = value;
            }
        }

        /// <remarks/>
        public int vatCategory
        {
            get
            {
                return this.vatCategoryField;
            }
            set
            {
                this.vatCategoryField = value;
            }
        }

        /// <remarks/>
        public decimal vatAmount
        {
            get
            {
                return this.vatAmountField;
            }
            set
            {
                this.vatAmountField = value;
            }
        }

        
        public byte? vatExemptionCategory
        {
            get
            {
                return this.vatExemptionCategoryField;
            }
            set
            {
                this.vatExemptionCategoryField = value;
            }
        }

        public bool ShouldSerializevatExemptionCategory()
        {
            // Return false if vatExemptionCategory is null, preventing serialization
            return vatCategory == 7;
        }

        /// <remarks/>
        public InvoicesDocInvoiceInvoiceDetailsDienergia dienergia
        {
            get
            {
                return this.dienergiaField;
            }
            set
            {
                this.dienergiaField = value;
            }
        }

        [XmlIgnore]
        public bool discountOption
        {
            get
            {
                return this.discountOptionField;
            }
            set
            {
                this.discountOptionField = value;
            }
        }

        public bool ShouldSerializewithheldAmount()
        {
            return withheldPercentCategory > 0;
        }

        public decimal withheldAmount
        {
            get
            {
                return this.withheldAmountField;
            }
            set
            {
                this.withheldAmountField = value;
            }
        }

        public bool ShouldSerializewithheldPercentCategory()
        {
            return withheldPercentCategory > 0;
        }
        public byte withheldPercentCategory
        {
            get
            {
                return this.withheldPercentCategoryField;
            }
            set
            {
                this.withheldPercentCategoryField = value;
            }
        }

         
        public decimal stampDutyAmount
        {
            get { return this.stampDutyAmountField; }
            set
            {
                {
                    this.stampDutyAmountField = value;
                }
            }
        }

        public bool ShouldSerializestampDutyAmount()
        {
            return stampDutyAmount > 0;
        }

      
        public int? stampDutyPercentCategory
        {
            get
            {
                return this.stampDutyPercentCategoryField;
            }
            set
            {
                this.stampDutyPercentCategoryField = value;
            }
        }

        public bool ShouldSerializestampDutyPercentCategory()
        {
            return stampDutyPercentCategory != null;
        }

        [XmlIgnore]
        public decimal feesAmount
        {
            get
            {
                return this.feesAmountField;
            }
            set
            {
                this.feesAmountField = value;
            }
        }

        [XmlIgnore]
        public byte feesPercentCategory
        {
            get
            {
                return this.feesPercentCategoryField;
            }
            set
            {
                this.feesPercentCategoryField = value;
            }
        }

        [XmlIgnore]
        public byte otherTaxesPercentCategory
        {
            get
            {
                return this.otherTaxesPercentCategoryField;
            }
            set
            {
                this.otherTaxesPercentCategoryField = value;
            }
        }

        [XmlIgnore]
        public decimal otherTaxesAmount
        {
            get
            {
                return this.otherTaxesAmountField;
            }
            set
            {
                this.otherTaxesAmountField = value;
            }
        }

        /// <remarks/>
        public decimal deductionsAmount
        {
            get
            {
                return this.deductionsAmountField;
            }
            set
            {
                this.deductionsAmountField = value;
            }
        }

        public bool ShouldSerializedeductionsAmount()
        {
            return deductionsAmount > 0;
        }

        /// <remarks/>
        public string lineComments
        {
            get
            {
                return this.lineCommentsField;
            }
            set
            {
                this.lineCommentsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("incomeClassification")]
        public InvoicesDocInvoiceInvoiceDetailsIncomeClassification[] incomeClassification
        {
            get
            {
                return this.incomeClassificationField;
            }
            set
            {
                this.incomeClassificationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("expensesClassification")]
        public InvoicesDocInvoiceInvoiceDetailsExpensesClassification[] expensesClassification
        {
            get
            {
                return this.expensesClassificationField;
            }
            set
            {
                this.expensesClassificationField = value;
            }
        }

        [XmlIgnore]
        public decimal quantity15
        {
            get
            {
                return this.quantity15Field;
            }
            set
            {
                this.quantity15Field = value;
            }
        }

        public int otherMeasurementUnitQuantity
        {
            get
            {
                return this.otherMeasurementUnitQuantityField;
            }
            set
            {
                this.otherMeasurementUnitQuantityField = value;
            }
        }

        public string otherMeasurementUnitTitle
        {
            get
            {
                return this.otherMeasurementUnitTitleField;
            }
            set
            {
                this.otherMeasurementUnitTitleField = value;
            }
        }
    }
}
