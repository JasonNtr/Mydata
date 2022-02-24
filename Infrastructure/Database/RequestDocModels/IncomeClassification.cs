using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    public class IncomeClassification : MyDataEntityDTO
    {
        [XmlElement(ElementName = "classificationType")]
        public virtual string classificationType { get; set; }
        [XmlElement(ElementName = "classificationCategory")]
        public virtual string classificationCategory { get; set; }
        [XmlElement(ElementName = "amount")]
        public virtual double? amount { get; set; }
        [XmlElement(ElementName = "optionalId")]
        public virtual int? optionalId { get; set; }
    }
}
