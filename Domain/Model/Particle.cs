using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("PARTICLE")]
    public class Particle
    {
        [Column("RECNUM")]
        public decimal Recnum { get; set; }

        [Key]
        [Column("COMPANY_CODE")]
        [StringLength(3)]
        public string Company { get; set; }

        [Key]
        [Column("BRANCH_CODE")]
        [StringLength(3)]
        public string Branch { get; set; }

        [Key]
        [Column("YEAR_YEAR")]
        public decimal Year { get; set; }

        [Key]
        [Column("CTYPKIN_CODE")]
        [StringLength(3)]
        public string CTYPKIN_CODE { get; set; }

        [Key]
        [Column("WTYPKIN_CODE")]
        [StringLength(3)]
        public string WTYPKIN_CODE { get; set; }

        [Key]
        [Column("PTYPPAR_CODE")]
        [StringLength(3)]
        public string PTYPPAR_CODE { get; set; }

        [Key]
        [Column("PSEIRA_SEIRA")]
        [StringLength(5)]
        public string Series { get; set; }


        [Key]
        [Column("CUSTPROM_CODE")]
        [StringLength(30)]
        public string CUSTPROM_CODE { get; set; }

        [Key]
        [Column("CLIENT_ID")]
        [StringLength(20)]
        public string ClientId { get; set; }

        [Key]
        [Column("PARTL_NO")]
        public decimal Number { get; set; }

        [Key]
        [Column("PARTL_HMNIA")]
        public DateTime Date { get; set; }

        [Column("PARTL_RECR")]
        [StringLength(12)]
        public string PARTL_RECR { get; set; }

        [Column("PTYPPAR_RECR")]
        [StringLength(12)]
        public string PTYPPAR_RECR { get; set; }

        [Column("TROPOIPL")]
        public decimal? Paymentmethod { get; set; }

        [Column("SKOPDIAK")]
        public decimal? TrasnferPurpose { get; set; }

        [Column("NOMISMA_CODE")]
        [StringLength(4)]
        public string CurrencyCode { get; set; }
        
        [Column("PARTL_MODULE")]
        [StringLength(4)]
        public string Module { get; set; }

        [Column("PARTL_PAYAM")]
        public decimal? Amount { get; set; }

        [Column("PARTL_TOTAMAD")]
        public decimal? TotalNetAmount { get; set; }

        [Column("PARTL_VATAM")]
        public decimal? TotalVatAmount { get; set; }

        [Column("PARTL_EXTRA")]
        public decimal? TotalOtherTaxesAmount { get; set; }

        [Column("PARTL_DISCAM")]
        public decimal? TotalDeductionsAmount { get; set; }


        [Column("PARAKRAT_POSO")]
       
        public decimal? WithHeldAmount { get; set; }

        [Column("AADE_MARK")]
        public string Mark { get; set; }
        [Column("SYSX_MARK")]
        public string CancelMark { get; set; }

        [Column("PARTL_EKLEISE")]
        public string Closed { get; set; }

        [Column("PARTL_EKTYPOSH")]
        public string Printed { get; set; }
        [Column("PARTL_REC0")]
        public decimal Rec0 { get; set; }
    }
}
