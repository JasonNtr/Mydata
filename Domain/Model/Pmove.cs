using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("PMOVES")]
    public class Pmove
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
        [Column("CUSTPROM_CODE")]
        [StringLength(30)]
        public string CUSTPROM_CODE { get; set; }

        [Key]
        [Column("CLIENT_ID")]
        [StringLength(20)]
        public string ClientId { get; set; }

        [Key]
        [Column("CTYPKIN_CODE")]
        [StringLength(3)]
        public string CTYPKIN_CODE { get; set; }


        [Key]
        [Column("WTYPKIN_CODE")]
        [StringLength(3)]
        public string WTYPKIN_CODE { get; set; }

        [Key]
        [Column("CN_CODE")]
        [StringLength(20)]
        public string ConstructionCode { get; set; }

        [Key]
        [Column("PTYPPAR_CODE")]
        [StringLength(3)]
        public string PTYPPAR_CODE { get; set; }

        [Key]
        [Column("PARTL_HMNIA")]
        public DateTime Date { get; set; }

        [Key]
        [Column("PSEIRA_SEIRA")]
        [StringLength(5)]
        public string Series { get; set; }

        [Key]
        [Column("PARTL_NO")]
        public decimal Number { get; set; }

        [Key]
        [Column("PMS_REC0")]
        public decimal PMS_REC0 { get; set; }

        [Column("PARTL_RECR")]
        [StringLength(12)]
        public string PARTL_RECR { get; set; }
        

        [Column("PMS_AMAFTDISC")]
        public decimal? PMS_AMAFTDISC { get; set; }

        [Column("PMS_VATAM")]
        public decimal PMS_VATAM { get; set; }

        [Column("POSO_PARAKRAT")]
        public decimal? POSO_PARAKRAT { get; set; }

        [Column("AADE_CODE_PARAK")]
        [StringLength(20)]
        public string AADE_CODE_PARAK { get; set; }

        [Column("POSO_XARTOSH")]
        public decimal? POSO_XARTOSH { get; set; }

        [Column("AADE_CODE_XARTO")]
        [StringLength(20)]
        public string AADE_CODE_XARTO { get; set; }

        [Column("PMS_DISCAM")]
        public decimal? PMS_DISCAM { get; set; }

        [Column("ITEM_CODE")]
        [StringLength(20)]
        public string ITEM_CODE { get; set; }

        [Column("PMS_VATPCT")]
        public decimal? PMS_VATPCT { get; set; }

    }
}
