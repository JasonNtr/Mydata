namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoiceInvoiceDetailsExpensesClassification
    {
        private string classificationTypeField;

        private string classificationCategoryField;

        private decimal amountField;

        private sbyte idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public string classificationType
        {
            get
            {
                return this.classificationTypeField;
            }
            set
            {
                this.classificationTypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
        public string classificationCategory
        {
            get
            {
                return this.classificationCategoryField;
            }
            set
            {
                this.classificationCategoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
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
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
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
