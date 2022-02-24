using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    [XmlRoot(ElementName = "RequestedDoc")]
    public class RequestedDoc
    {
        [XmlElement(ElementName = "invoicesDoc")]
        public virtual InvoicesDoc invoicesDoc { get; set; }

        [XmlElement(ElementName = "cancelledInvoicesDoc")]
        public virtual CancelledInvoicesDoc cancelledInvoicesDoc { get; set; }

        [XmlElement(ElementName = "continuationToken")]
        public virtual ContinuationToken continuationToken { get; set; }
    }
}
