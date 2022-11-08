namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoicePaymentMethodDetails
    {
        private int typeField;

        private decimal amountField;

        private string paymentMethodInfoField;

        /// <remarks/>
        public int type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public decimal amount
        {
            get
            {
                return this.amountField;
            }
            set
            {
                this.amountField = value;
            }
        }

        /// <remarks/>
        public string paymentMethodInfo
        {
            get
            {
                return this.paymentMethodInfoField;
            }
            set
            {
                this.paymentMethodInfoField = value;
            }
        }
    }
}
