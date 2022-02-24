using Domain.DTO;


namespace Infrastructure.Database.RequestDocModels
{
    public class AddressType : MyDataEntityDTO
    {
        public virtual string postalCode { get; set; }
        public virtual string city { get; set; }
        public virtual string street { get; set; }
        public virtual string number { get; set; }

    }
}
