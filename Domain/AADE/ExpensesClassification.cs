using static Domain.Enums.Enums;

namespace Domain.AADE
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0", IsNullable = false)]
    public partial class ExpensesClassificationsDoc
    {
        private InvoiceExpensesClassificationType[] expensesInvoiceClassificationField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("expensesInvoiceClassification")]
        public InvoiceExpensesClassificationType[] expensesInvoiceClassification
        {
            get
            {
                return this.expensesInvoiceClassificationField;
            }
            set
            {
                this.expensesInvoiceClassificationField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public partial class InvoiceExpensesClassificationType
    {
        private long invoiceMarkField;

        private long classificationMarkField;

        private bool classificationMarkFieldSpecified;

        private string entityVatNumberField;

        private object[] itemsField;

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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("invoicesExpensesClassificationDetails", typeof(InvoicesExpensesClassificationDetailType))]
        [System.Xml.Serialization.XmlElementAttribute("transactionMode", typeof(int))]
        public object[] Items
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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public partial class InvoicesExpensesClassificationDetailType
    {
        private int lineNumberField;

        private ExpensesClassificationType[] expensesClassificationDetailDataField;

        /// <remarks/>
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("expensesClassificationDetailData")]
        public ExpensesClassificationType[] expensesClassificationDetailData
        {
            get
            {
                return this.expensesClassificationDetailDataField;
            }
            set
            {
                this.expensesClassificationDetailDataField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "https://www.aade.gr/myDATA/expensesClassificaton/v1.0")]
    public partial class ExpensesClassificationType
    {
        private ExpensesClassificationTypeClassificationType classificationTypeField;

        private bool classificationTypeFieldSpecified;

        private ExpensesClassificationCategoryType classificationCategoryField;

        private bool classificationCategoryFieldSpecified;

        private decimal amountField;

        private sbyte idField;

        private bool idFieldSpecified;

        /// <remarks/>
        public ExpensesClassificationTypeClassificationType classificationType
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

        /// <remarks/>
        public ExpensesClassificationCategoryType classificationCategory
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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool classificationCategorySpecified
        {
            get
            {
                return this.classificationCategoryFieldSpecified;
            }
            set
            {
                this.classificationCategoryFieldSpecified = value;
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

        /// <remarks/>
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