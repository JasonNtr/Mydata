using System.Xml.Serialization;


namespace Infrastructure.Database.RequestDocModels
{
    [XmlRoot(ElementName = "taxesTotals")]
    public class TaxesTotals
    {
        [XmlElement(ElementName = "taxes")]
        public virtual Taxes? taxes { get; set; }
    }
}
