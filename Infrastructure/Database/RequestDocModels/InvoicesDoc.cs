using Domain.DTO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    [XmlRoot(ElementName = "invoicesDoc")]
    public class InvoicesDoc : MyDataEntityDTO
    {
        [XmlElement(ElementName = "invoice")]
        public virtual List<Invoice> invoice { get; set; } = new List<Invoice>();
    }
}
