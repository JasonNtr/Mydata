using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AADE
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0", IsNullable = false)]
    public partial class IncomeClassificationsDoc
    {

        private IncomeClassificationsDocIncomeInvoiceClassification[] incomeInvoiceClassificationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("incomeInvoiceClassification")]
        public IncomeClassificationsDocIncomeInvoiceClassification[] incomeInvoiceClassification
        {
            get
            {
                return this.incomeInvoiceClassificationField;
            }
            set
            {
                this.incomeInvoiceClassificationField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public partial class IncomeClassificationsDocIncomeInvoiceClassification
    {

        private long invoiceMarkField;

        private long classificationMarkField;

        private string entityVatNumberField;
        private byte transactionModeField;

        private IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetails[] invoicesIncomeClassificationDetailsField;

        

        private bool transactionModeFieldSpecified;

        /// <remarks/>
        public long invoiceMark
        {
            get
            {
                return this.invoiceMarkField;
            }
            set
            {
                this.invoiceMarkField = value;
            }
        }

        /// <remarks/>
        public long classificationMark
        {
            get
            {
                return this.classificationMarkField;
            }
            set
            {
                this.classificationMarkField = value;
            }
        }

        /// <remarks/>
        public string entityVatNumber
        {
            get
            {
                return this.entityVatNumberField;
            }
            set
            {
                this.entityVatNumberField = value;
            }
        }

        public byte transactionMode
        {
            get
            {
                return this.transactionModeField;
            }
            set
            {
                this.transactionModeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("invoicesIncomeClassificationDetails")]
        public IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetails[] invoicesIncomeClassificationDetails
        {
            get
            {
                return this.invoicesIncomeClassificationDetailsField;
            }
            set
            {
                this.invoicesIncomeClassificationDetailsField = value;
            }
        }

        /// <remarks/>
        

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool transactionModeSpecified
        {
            get
            {
                return this.transactionModeFieldSpecified;
            }
            set
            {
                this.transactionModeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public partial class IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetails
    {

        private short lineNumberField;

        private IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetailsIncomeClassificationDetailData incomeClassificationDetailDataField;

        /// <remarks/>
        public short lineNumber
        {
            get
            {
                return this.lineNumberField;
            }
            set
            {
                this.lineNumberField = value;
            }
        }

        /// <remarks/>
        public IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetailsIncomeClassificationDetailData incomeClassificationDetailData
        {
            get
            {
                return this.incomeClassificationDetailDataField;
            }
            set
            {
                this.incomeClassificationDetailDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public partial class IncomeClassificationsDocIncomeInvoiceClassificationInvoicesIncomeClassificationDetailsIncomeClassificationDetailData
    {

        private string classificationCategoryField;

        private decimal amountField;

        /// <remarks/>
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
    }



}

