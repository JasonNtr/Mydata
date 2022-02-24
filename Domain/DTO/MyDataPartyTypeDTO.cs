using System;

namespace Domain.DTO
{
    public class MyDataPartyTypeDTO : MyDataEntityDTO
    {
        public virtual Guid? MyDataDocEncounterInvoiceId { get; set; }
        public virtual MyDataTransmittedDocInvoiceDTO MyDataDocEncounterInvoice { get; set; }
        public virtual Guid? MyDataDocIssuerInvoiceId { get; set; }
        public virtual MyDataTransmittedDocInvoiceDTO MyDataDocIssuerInvoice { get; set; }
        public virtual string vatNumber { get; set; }
        public virtual string country { get; set; }
        public virtual string branch { get; set; }
        public virtual string name { get; set; }
        public virtual MyDataAddressTypeDTO? address { get; set; }
    }
}
