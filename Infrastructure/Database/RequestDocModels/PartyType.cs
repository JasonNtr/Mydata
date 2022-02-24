

using Domain.DTO;

namespace Infrastructure.Database.RequestDocModels
{
    public class PartyType : MyDataEntityDTO
    {
        //public virtual Guid? MyDataDocEncounterInvoiceId { get; set; }
        //public virtual DocInvoice MyDataDocEncounterInvoice { get; set; }
        //public virtual Guid? MyDataDocIssuerInvoiceId { get; set; }
        //public virtual MyDataDocInvoiceDTO MyDataDocIssuerInvoice { get; set; }

        public virtual string vatNumber { get; set; }
        public virtual string country { get; set; }
        public virtual string branch { get; set; }
        public virtual string name { get; set; }
        public virtual AddressType? address { get; set; }
    }
}
