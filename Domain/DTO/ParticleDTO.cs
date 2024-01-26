using System;
using System.Collections.Generic;

namespace Domain.DTO
{
    public class ParticleDTO
    {
        public DateTime Date { get; set; }
         public decimal Number { get; set; }
        public decimal Code { get; set; }

        public decimal InvoiceType { get; set; }

         public decimal? Paymentmethod { get; set; }

        public decimal ClientCode { get; set; }
        public decimal ShipCode { get; set; }

        public decimal Total { get; set; }
        public string? CancelMark { get; set; }
        public decimal? Amount { get; set; }

        public string Currency { get; set; }
        public decimal? CancelledBy { get; set; }
        public decimal? CanceledParticle { get; set; }

        public Guid DataGridId { get; set; }
        public string? Mark { get; set; }
        public string? Closed { get; set; }

        public virtual InvoiceTypeDTO Ptyppar { get; set; }

        public virtual List<PMoveDTO> Pmoves { get; set; }
        public virtual ClientDTO Client { get; set; }
    }
}
