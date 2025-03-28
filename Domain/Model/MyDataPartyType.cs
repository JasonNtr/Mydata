﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class MyDataPartyType : MyDataEntity
    {
        public virtual Guid? MyDataDocIssuerInvoiceId { get; set; }
        public virtual MyDataTransmittedDocInvoice MyDataDocIssuerInvoice { get; set; }

        public virtual Guid? MyDataDocEncounterInvoiceId { get; set; }
        public virtual MyDataTransmittedDocInvoice MyDataDocEncounterInvoice { get; set; }

        [StringLength(50)]
        public virtual string vatNumber { get; set; }
        [StringLength(50)]
        public virtual string country { get; set; }
        [StringLength(20)]
        public virtual string branch { get; set; }
        [StringLength(50)]
        public virtual string name { get; set; }
        public virtual MyDataAddressType? address { get; set; }

    }
}
