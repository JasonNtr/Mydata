namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ResponseDoc
    {

        private ResponseDocResponse[] responseField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("response")]
        public ResponseDocResponse[] response
        {
            get { return this.responseField; }
            set { this.responseField = value; }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class ResponseDocResponse
    {
        private ResponseDocResponseError[] errorsField;

        private int indexField;

        private string invoiceUidField;

        private long invoiceMarkField;

        private long classificationMarkField;

        private long cancellationMarkField;

        private string authenticationCodeField;

        private string[][] receptionProvidersField;

        private string[] receptionEmailsField;

        private string statusCodeField;
        private string qrUrlField;

        /// <remarks/>
        public int index
        {
            get { return this.indexField; }
            set { this.indexField = value; }
        }

        /// <remarks/>
        public string invoiceUid
        {
            get { return this.invoiceUidField; }
            set { this.invoiceUidField = value; }
        }

        /// <remarks/>
        public long invoiceMark
        {
            get { return this.invoiceMarkField; }
            set { this.invoiceMarkField = value; }
        }

        /// <remarks/>
        public long classificationMark
        {
            get { return this.classificationMarkField; }
            set { this.classificationMarkField = value; }
        }

        /// <remarks/>
        public long cancellationMark
        {
            get { return this.cancellationMarkField; }
            set { this.cancellationMarkField = value; }
        }

        /// <remarks/>
        public string authenticationCode
        {
            get { return this.authenticationCodeField; }
            set { this.authenticationCodeField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ProviderInfo", IsNullable = false)]
        [System.Xml.Serialization.XmlArrayItemAttribute("VATNumber", IsNullable = false, NestingLevel = 1)]
        public string[][] receptionProviders
        {
            get { return this.receptionProvidersField; }
            set { this.receptionProvidersField = value; }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("email", IsNullable = false)]
        public string[] receptionEmails
        {
            get { return this.receptionEmailsField; }
            set { this.receptionEmailsField = value; }
        }

        /// <remarks/>
        public string statusCode
        {
            get { return this.statusCodeField; }
            set { this.statusCodeField = value; }
        }

        public string qrUrl
        {
            get { return this.qrUrlField; }
            set { this.qrUrlField = value; }
        }

        [System.Xml.Serialization.XmlArrayItemAttribute("error", IsNullable = false)]
        public ResponseDocResponseError[] errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
            }
        }
    }
}
