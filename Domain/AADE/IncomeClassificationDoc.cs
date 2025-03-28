﻿namespace Domain.AADE
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0", IsNullable = false)]
    public partial class IncomeClassificationsDoc
    {
       
        private InvoiceIncomeClassificationType[] incomeInvoiceClassificationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("incomeInvoiceClassification")]
        public InvoiceIncomeClassificationType[] incomeInvoiceClassification
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

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public partial class InvoiceIncomeClassificationType
    {
        private long invoiceMarkField;

        private long classificationMarkField;

        private bool classificationMarkFieldSpecified;

        private string entityVatNumberField;
        private byte transactionModeField;
        private bool transactionModeFieldSpecified;

        private InvoicesIncomeClassificationDetailType[] itemsField;

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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classificationMarkSpecified
        {
            get
            {
                return this.classificationMarkFieldSpecified;
            }
            set
            {
                this.classificationMarkFieldSpecified = value;
            }
        }

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

        [System.Xml.Serialization.XmlElementAttribute("invoicesIncomeClassificationDetails")]
        public InvoicesIncomeClassificationDetailType[] invoicesIncomeClassificationDetails
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    public partial class InvoicesIncomeClassificationDetailType
    {
        private int lineNumberField;

        private IncomeClassificationType[] incomeClassificationDetailDataField;

        public int lineNumber
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

        [System.Xml.Serialization.XmlElementAttribute("incomeClassificationDetailData")]
        public IncomeClassificationType[] incomeClassificationDetailData
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

    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/incomeClassificaton/v1.0")]
    
    public partial class IncomeClassificationType
    {
        private Enums.Enums.IncomeClassificationValueType classificationTypeField;

        private bool classificationTypeFieldSpecified;

        private Enums.Enums.IncomeClassificationCategoryType classificationCategoryField;

        private decimal amountField;

        private sbyte idField;

        private bool idFieldSpecified;

        public Enums.Enums.IncomeClassificationValueType classificationType
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classificationTypeSpecified
        {
            get
            {
                return this.classificationTypeFieldSpecified;
            }
            set
            {
                this.classificationTypeFieldSpecified = value;
            }
        }

        public Enums.Enums.IncomeClassificationCategoryType classificationCategory
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

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool idSpecified
        {
            get
            {
                return this.idFieldSpecified;
            }
            set
            {
                this.idFieldSpecified = value;
            }
        }
    }
}