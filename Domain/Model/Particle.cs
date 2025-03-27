using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("INVHDR")]

    public class Particle
    {
        [Column("DATE")]
        public DateTime Date { get; set; }
        [Column("NUMBER")]
        public decimal Number { get; set; }
        [Column("CODE")]
     
        public decimal Code { get; set; }
         
       

        [Column("CANCELLED_BY_CODE")]
        public decimal? CancelledBy { get; set; }

        [Column("CANCELLING_INVOICE_CODE")]
        public decimal? CanceledParticle { get; set; }
        [Column("CANCELLING_INVOICE_CODE")]
        public decimal? CancelInvoiceCode { get; set; }
        [Column("PARTL_PAYAM")]
        public decimal? Amount { get; set; }

        [Column("INVTYPE")]
        public decimal InvoiceType { get; set; }

        [Column("TROPOIPL")]
        public decimal? Paymentmethod { get; set; }

        
        [Column("GRAND_TOTAL")]
        public decimal Total { get; set; }

        [Column("CURRENCY")]
       
        public string Currency { get; set; }

        
        [Column("QRCSTRING")]
        public string QrCode { get; set; }
        [Column("AADE_UID")]
        public string AadeUid { get; set; }
        [Column("SUBCOMP")]
     
        public decimal ShipCode { get; set; }
        [Column("VAT_AMOUNT")]
        public decimal VatAmount { get; set; }

        [Column("VAT_PERCENTAGE")]
        public decimal VatPercentage{ get; set; }

        [Column("MAINCOMP")]
        public decimal ClientCode { get; set; }

        [Column("AADE_MARK")]
     
        
        public string? Mark { get; set; }

        [Column("SYSX_MARK")]
        public string? CancelMark { get; set; }


        [Column("PARTL_EKLEISE")]
        public string? Closed { get; set; }

        [Column("DispatchTime")]
        public string? Time { get; set; }


        [Column("DispatchDate")]
        public DateTime? DispatchDate { get; set; }

        public string? VehiculeNumber { get; set; }
        public short? MovePurpose { get; set; }

        public string? LoadingStreet { get; set; }

        public string? LoadingNumber { get; set; }

        public string? LoadingPostalCode { get; set; }

        public string? LoadingCity { get; set; }

        public string? DeliveryStreet { get; set; }

        public string? DeliveryNumber { get; set; }

        public string? DeliveryPostalCode { get; set; }
     
        public string? DeliveryCity { get; set; }
        public decimal? SKOPDIAK { get; set; }

        [Column("EPILOGH_EKKREM")]
        public byte? TransmissionFailure { get; set; }
    }
}
