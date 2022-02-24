
using Domain.DTO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    [XmlRoot(ElementName = "RequestedDoc")]
    public class PaymentMethod : MyDataEntityDTO
    {
        [XmlElement(ElementName = "paymentMethodDetails")]
        public virtual List<PaymentMethodDetail> paymentMethodDetails { get; set; } = new List<PaymentMethodDetail>();
    }
}
