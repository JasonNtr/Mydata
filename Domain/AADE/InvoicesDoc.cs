namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.aade.gr/myDATA/invoice/v1.0", IsNullable = false)]
    public partial class InvoicesDoc
    {
        private InvoicesDocInvoice[] invoiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("invoice")]
        public InvoicesDocInvoice[] invoice
        {
            get
            {
                return this.invoiceField;
            }
            set
            {
                this.invoiceField = value;
            }
        }
    }
}
