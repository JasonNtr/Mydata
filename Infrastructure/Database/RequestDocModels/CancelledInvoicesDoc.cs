using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    [XmlRoot(ElementName = "cancelledInvoicesDoc")]
    public class CancelledInvoicesDoc : MyDataEntityDTO
    {
        [XmlElement(ElementName = "cancelledInvoice")]
        public virtual List<CancelledInvoice> cancelledInvoice { get; set; } = new List<CancelledInvoice>();
    }
}
