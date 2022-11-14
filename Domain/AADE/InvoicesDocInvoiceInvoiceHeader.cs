using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoiceInvoiceHeader
    {
        private string seriesField;

        private string aaField;

        private System.DateTime issueDateField;

        private decimal invoiceTypeField;

        private bool vatPaymentSuspensionField;

        private string currencyField;

        private decimal exchangeRateField;

        private long[] correlatedInvoicesField;

        private bool selfPricingField;

         

        private string vehicleNumberField;

      

        private bool fuelInvoiceField;

        private byte specialInvoiceCategoryField;

        private byte invoiceVariationTypeField;

        /// <remarks/>
        public string series
        {
            get
            {
                return this.seriesField;
            }
            set
            {
                this.seriesField = value;
            }
        }

        /// <remarks/>
        public string aa
        {
            get
            {
                return this.aaField;
            }
            set
            {
                this.aaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime issueDate
        {
            get
            {
                return this.issueDateField;
            }
            set
            {
                this.issueDateField = value;
            }
        }

        /// <remarks/>
        public decimal invoiceType
        {
            get
            {
                return this.invoiceTypeField;
            }
            set
            {
                this.invoiceTypeField = value;
            }
        }

        /// <remarks/>
        public bool vatPaymentSuspension
        {
            get
            {
                return this.vatPaymentSuspensionField;
            }
            set
            {
                this.vatPaymentSuspensionField = value;
            }
        }

        /// <remarks/>
        public string currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        [XmlIgnore]
        public string exchangeRate
        {
            get
            {
                return this.exchangeRateField.ToString("c");
            }
            set
            {
                this.exchangeRateField = Convert.ToDecimal(value);
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("correlatedInvoices")]
        public long[] correlatedInvoices
        {
            get
            {
                return this.correlatedInvoicesField;
            }
            set
            {
                this.correlatedInvoicesField = value;
            }
        }

        [XmlIgnore]
        public bool selfPricing
        {
            get
            {
                return this.selfPricingField;
            }
            set
            {
                this.selfPricingField = value;
            }
        }

        

        /// <remarks/>
        public string vehicleNumber
        {
            get
            {
                return this.vehicleNumberField;
            }
            set
            {
                this.vehicleNumberField = value;
            }
        }

     

        [XmlIgnore]
        public bool fuelInvoice
        {
            get
            {
                return this.fuelInvoiceField;
            }
            set
            {
                this.fuelInvoiceField = value;
            }
        }

        [XmlIgnore]
        public byte specialInvoiceCategory
        {
            get
            {
                return this.specialInvoiceCategoryField;
            }
            set
            {
                this.specialInvoiceCategoryField = value;
            }
        }

        [XmlIgnore]
        public byte invoiceVariationType
        {
            get
            {
                return this.invoiceVariationTypeField;
            }
            set
            {
                this.invoiceVariationTypeField = value;
            }
        }
    }
}
