using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    public class ContinuationToken : MyDataEntityDTO
    {
        [XmlElement(ElementName = "nextPartitionKey")]
        public virtual string? nextPartitionKey { get; set; }
        [XmlElement(ElementName = "nextRowKey")]
        public virtual string? nextRowKey { get; set; }
    }
}
