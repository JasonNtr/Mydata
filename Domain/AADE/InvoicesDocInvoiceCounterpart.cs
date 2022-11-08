namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoiceCounterpart
    {
        private string vatNumberField;

        private string countryField;

        private int branchField;

        private string nameField;

        private InvoicesDocInvoiceCounterpartAddress addressField;

        private string documentIdNoField;

        private string supplyAccountNoField;

        /// <remarks/>
        public string vatNumber
        {
            get
            {
                return this.vatNumberField;
            }
            set
            {
                this.vatNumberField = value;
            }
        }

        /// <remarks/>
        public string country
        {
            get
            {
                return this.countryField;
            }
            set
            {
                this.countryField = value;
            }
        }

        /// <remarks/>
        public int branch
        {
            get
            {
                return this.branchField;
            }
            set
            {
                this.branchField = value;
            }
        }

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public InvoicesDocInvoiceCounterpartAddress address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        public string documentIdNo
        {
            get
            {
                return this.documentIdNoField;
            }
            set
            {
                this.documentIdNoField = value;
            }
        }

        /// <remarks/>
        public string supplyAccountNo
        {
            get
            {
                return this.supplyAccountNoField;
            }
            set
            {
                this.supplyAccountNoField = value;
            }
        }
    }
}
