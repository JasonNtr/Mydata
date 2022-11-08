namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoiceInvoiceSummary
    {
        private decimal totalNetValueField;

        private decimal totalVatAmountField;

        private decimal totalWithheldAmountField;

        private decimal totalFeesAmountField;

        private decimal totalStampDutyAmountField;

        private decimal totalOtherTaxesAmountField;

        private decimal totalDeductionsAmountField;

        private decimal totalGrossValueField;

        private InvoicesDocInvoiceInvoiceSummaryIncomeClassification[] incomeClassificationField;

        private InvoicesDocInvoiceInvoiceSummaryExpensesClassification[] expensesClassificationField;

        /// <remarks/>
        public decimal totalNetValue
        {
            get
            {
                return this.totalNetValueField;
            }
            set
            {
                this.totalNetValueField = value;
            }
        }

        /// <remarks/>
        public decimal totalVatAmount
        {
            get
            {
                return this.totalVatAmountField;
            }
            set
            {
                this.totalVatAmountField = value;
            }
        }

        /// <remarks/>
        public decimal totalWithheldAmount
        {
            get
            {
                return this.totalWithheldAmountField;
            }
            set
            {
                this.totalWithheldAmountField = value;
            }
        }

        /// <remarks/>
        public decimal totalFeesAmount
        {
            get
            {
                return this.totalFeesAmountField;
            }
            set
            {
                this.totalFeesAmountField = value;
            }
        }

        /// <remarks/>
        public decimal totalStampDutyAmount
        {
            get
            {
                return this.totalStampDutyAmountField;
            }
            set
            {
                this.totalStampDutyAmountField = value;
            }
        }

        /// <remarks/>
        public decimal totalOtherTaxesAmount
        {
            get
            {
                return this.totalOtherTaxesAmountField;
            }
            set
            {
                this.totalOtherTaxesAmountField = value;
            }
        }

        /// <remarks/>
        public decimal totalDeductionsAmount
        {
            get
            {
                return this.totalDeductionsAmountField;
            }
            set
            {
                this.totalDeductionsAmountField = value;
            }
        }

        /// <remarks/>
        public decimal totalGrossValue
        {
            get
            {
                return this.totalGrossValueField;
            }
            set
            {
                this.totalGrossValueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("incomeClassification")]
        public InvoicesDocInvoiceInvoiceSummaryIncomeClassification[] incomeClassification
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
        public InvoicesDocInvoiceInvoiceSummaryExpensesClassification[] expensesClassification
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
    }

}
