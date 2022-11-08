namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoiceInvoiceDetailsDienergia
    {
        private string applicationIdField;

        private System.DateTime applicationDateField;

        private string doyField;

        private string shipIdField;

        /// <remarks/>
        public string applicationId
        {
            get
            {
                return this.applicationIdField;
            }
            set
            {
                this.applicationIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime applicationDate
        {
            get
            {
                return this.applicationDateField;
            }
            set
            {
                this.applicationDateField = value;
            }
        }

        /// <remarks/>
        public string doy
        {
            get
            {
                return this.doyField;
            }
            set
            {
                this.doyField = value;
            }
        }

        /// <remarks/>
        public string shipId
        {
            get
            {
                return this.shipIdField;
            }
            set
            {
                this.shipIdField = value;
            }
        }
    }
}
