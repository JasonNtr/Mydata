namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoiceTaxes
    {
        private byte taxTypeField;

        private uint taxCategoryField;

        private decimal underlyingValueField;

        private decimal taxAmountField;

        private sbyte idField;

        /// <remarks/>
        public byte taxType
        {
            get
            {
                return this.taxTypeField;
            }
            set
            {
                this.taxTypeField = value;
            }
        }

        /// <remarks/>
        public uint taxCategory
        {
            get
            {
                return this.taxCategoryField;
            }
            set
            {
                this.taxCategoryField = value;
            }
        }

        /// <remarks/>
        public decimal underlyingValue
        {
            get
            {
                return this.underlyingValueField;
            }
            set
            {
                this.underlyingValueField = value;
            }
        }

        /// <remarks/>
        public decimal taxAmount
        {
            get
            {
                return this.taxAmountField;
            }
            set
            {
                this.taxAmountField = value;
            }
        }

        /// <remarks/>
        public sbyte id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }
}
