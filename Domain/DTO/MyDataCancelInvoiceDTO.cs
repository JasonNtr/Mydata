using Domain.Model;
using System;
using System.Collections.Generic;

namespace Domain.DTO
{
    public class MyDataCancelInvoiceDTO
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual long? Uid { get; set; }
        public virtual long? invoiceMark { get; set; }
        public virtual long? ParticleToBeCancelledMark { get; set; }
        public virtual bool invoiceProcessed { get; set; }

        
        public ParticleDTO Particle { get; set; }
    }
}