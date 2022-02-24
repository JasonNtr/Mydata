using Domain.DTO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    [XmlRoot(ElementName = "invoice")]
    public class Invoice : MyDataEntityDTO
    {
        [XmlElement(ElementName = "uid")]
        public virtual string? Uid { get; set; }
        public virtual string? authenticationCode { get; set; }
        public virtual long? mark { get; set; }
        [XmlElement(ElementName = "issuer")]
        public virtual List<PartyType> issuer { get; set; } = new List<PartyType>();
        [XmlElement(ElementName = "counterpart")]
        public virtual List<PartyType> counterpart { get; set; } = new List<PartyType>();
        [XmlElement(ElementName = "invoiceHeader")]
        public virtual InvoiceHeaderType invoiceHeader { get; set; }
        [XmlElement(ElementName = "paymentMethods")]
        public virtual PaymentMethod paymentMethods { get; set; }
        [XmlElement(ElementName = "invoiceDetails")]
        public virtual List<InvoiceRowType> invoiceDetails { get; set; } = new List<InvoiceRowType>();
        [XmlElement(ElementName = "taxesTotals")]
        public virtual List<TaxesTotals> taxesTotals { get; set; } = new List<TaxesTotals>();
        [XmlElement(ElementName = "invoiceSummary")]
        public virtual InvoiceSummaryType invoiceSummary { get; set; }
    }
}
