using System;

namespace Domain.DTO
{
    public class MyDataAddressTypeDTO : MyDataEntityDTO
    {
        public virtual MyDataPartyTypeDTO MyDataPartyType { get; set; }
        public virtual Guid MyDataPartyTypeId { get; set; }
        public virtual string postalCode { get; set; }
        public virtual string city { get; set; }
        public virtual string street { get; set; }
        public virtual string number { get; set; }
    }
}
