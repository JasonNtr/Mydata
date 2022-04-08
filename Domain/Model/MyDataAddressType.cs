using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataAddressType : MyDataEntity
    {
        
        public virtual MyDataPartyType MyDataPartyType { get; set; }
        [ForeignKey("MyDataPartyTypeId")]
        public virtual Guid MyDataPartyTypeId { get; set; }
        [StringLength(50)]
        public virtual string postalCode { get; set; }
        [StringLength(50)]
        public virtual string city { get; set; }
        [StringLength(50)]
        public virtual string street { get; set; }
        [StringLength(50)]
        public virtual string number { get; set; }
    }
}
