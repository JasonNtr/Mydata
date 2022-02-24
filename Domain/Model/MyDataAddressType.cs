using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataAddressType : MyDataEntity
    {
        
        public virtual MyDataPartyType MyDataPartyType { get; set; }
        [ForeignKey("MyDataPartyTypeId")]
        public virtual Guid MyDataPartyTypeId { get; set; }
        public virtual string postalCode { get; set; }
        public virtual string city { get; set; }
        public virtual string street { get; set; }
        public virtual string number { get; set; }
    }
}
