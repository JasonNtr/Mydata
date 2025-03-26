using System.Xml.Serialization;

namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.aade.gr/myDATA/invoice/v1.0")]
    public partial class InvoicesDocInvoice
    {
        private string uidField;

        private long markField;

        private long cancelledByMarkField;

        private string authenticationCodeField;

        private byte transmissionFailureField;

        private InvoicesDocInvoiceIssuer issuerField;

        private InvoicesDocInvoiceCounterpart counterpartField;

        private InvoicesDocInvoiceInvoiceHeader invoiceHeaderField;

        private InvoicesDocInvoicePaymentMethodDetails[] paymentMethodsField;

        private InvoicesDocInvoiceInvoiceDetails[] invoiceDetailsField;

        private InvoicesDocInvoiceTaxes[] taxesTotalsField;

        private InvoicesDocInvoiceInvoiceSummary invoiceSummaryField;

        /// <remarks/>
        public string uid
        {
            get
            {
                return this.uidField;
            }
            set
            {
                this.uidField = value;
            }
        }

        /// <remarks/>
        public long mark
        {
            get
            {
                return this.markField;
            }
            set
            {
                this.markField = value;
            }
        }

        /// <remarks/>
        public long cancelledByMark
        {
            get
            {
                return this.cancelledByMarkField;
            }
            set
            {
                this.cancelledByMarkField = value;
            }
        }

        /// <remarks/>
        public string authenticationCode
        {
            get
            {
                return this.authenticationCodeField;
            }
            set
            {
                this.authenticationCodeField = value;
            }
        }

       
        public byte transmissionFailure
        {
            get
            {
                return this.transmissionFailureField;
            }
            set
            {
                this.transmissionFailureField = value;
            }
        }

        public bool ShouldSerializetransmissionFailure()
        {
            return transmissionFailure > 0;
        }

        /// <remarks/>
        public InvoicesDocInvoiceIssuer issuer
        {
            get
            {
                return this.issuerField;
            }
            set
            {
                this.issuerField = value;
            }
        }

        /// <remarks/>
        public InvoicesDocInvoiceCounterpart counterpart
        {
            get
            {
                return this.counterpartField;
            }
            set
            {
                this.counterpartField = value;
            }
        }

        /// <remarks/>
        public InvoicesDocInvoiceInvoiceHeader invoiceHeader
        {
            get
            {
                return this.invoiceHeaderField;
            }
            set
            {
                this.invoiceHeaderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("paymentMethodDetails", IsNullable = false)]
        public InvoicesDocInvoicePaymentMethodDetails[] paymentMethods
        {
            get
            {
                return this.paymentMethodsField;
            }
            set
            {
                this.paymentMethodsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("invoiceDetails")]
        public InvoicesDocInvoiceInvoiceDetails[] invoiceDetails
        {
            get
            {
                return this.invoiceDetailsField;
            }
            set
            {
                this.invoiceDetailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("taxes", IsNullable = false)]
        public InvoicesDocInvoiceTaxes[] taxesTotals
        {
            get
            {
                return this.taxesTotalsField;
            }
            set
            {
                this.taxesTotalsField = value;
            }
        }

        /// <remarks/>
        public InvoicesDocInvoiceInvoiceSummary invoiceSummary
        {
            get
            {
                return this.invoiceSummaryField;
            }
            set
            {
                this.invoiceSummaryField = value;
            }
        }
    }
}
