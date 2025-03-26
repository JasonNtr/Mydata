using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string QrCode { get; set; }
        public string AadeUid { get; set; }

        public string Currency { get; set; }
        public decimal? CancelledBy { get; set; }
        public decimal? CanceledParticle { get; set; }
        public decimal? CancelInvoiceCode { get; set; }
        public decimal VatAmount { get; set; }

        
        public decimal VatPercentage { get; set; }
        public Guid DataGridId { get; set; }
        public string? Mark { get; set; }
        public string? Closed { get; set; }

        public string? Time { get; set; }

        public string? VehiculeNumber { get; set; }
        public int? MovePurpose { get; set; }

        public string? LoadingStreet { get; set; }

        public string? LoadingNumber { get; set; }

        public string? LoadingPostalCode { get; set; }

        public string? LoadingCity { get; set; }

        public string? DeliveryStreet { get; set; }

        public string? DeliveryNumber { get; set; }

        public string? DeliveryPostalCode { get; set; }
        public decimal SKOPDIAK { get; set; }
        public DateTime? DispatchDate { get; set; }
        public string? DeliveryCity { get; set; }
      
        public decimal? TransmissionFailure { get; set; }
        public virtual MovePurposeDTO MovePurposeDTO { get; set; }

        public virtual InvoiceTypeDTO Ptyppar { get; set; }
        public virtual ItemDTO ItemDTO { get; set; }

        public virtual List<PMoveDTO> Pmoves { get; set; }
        public virtual ClientDTO Client { get; set; }
    }
}
