using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    public class Taxes : MyDataEntityDTO
    {
        [XmlElement(ElementName = "taxType")]
        public virtual int? taxType { get; set; }
        [XmlElement(ElementName = "taxCategory")]
        public virtual int? taxCategory { get; set; }
        [XmlElement(ElementName = "underlyingValue")]
        public virtual double? taxunderlyingValueType { get; set; }
        [XmlElement(ElementName = "taxAmount")]
        public virtual double? taxAmount { get; set; }
        //<taxes>
        //  <taxType>1</taxType>
        //<taxCategory>1</taxCategory>
        //  <underlyingValue>479.53</underlyingValue>
        //  <taxAmount>115.08</taxAmount>
        //</taxes>
    }
}
