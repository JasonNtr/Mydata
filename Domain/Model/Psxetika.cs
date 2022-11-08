using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("PSXETIKA")]
    public class Psxetika
    {
        public decimal RECNUM { get; set; }
        [Key]
        [StringLength(3)]
        public string COMPANY_CODE { get; set; }
        [Key]
        [StringLength(3)]
        public string BRANCH_CODE { get; set; }
        [Key]
        public decimal YEAR_YEAR { get; set; }
        [Key]
        [StringLength(3)]
        public string CTYPKIN_CODE { get; set; }
        [Key]
        [StringLength(3)]
        public string WTYPKIN_CODE { get; set; }
        [Key]
        [StringLength(3)]
        public string PTYPPAR_CODE { get; set; }
        [Key]
        [StringLength(5)]
        public string PSEIRA_SEIRA { get; set; }
        [Key]
        [StringLength(30)]
        public string CUSTPROM_CODE { get; set; }
        [Key]
        [StringLength(20)]
        public string CLIENT_ID { get; set; }
        [Key]
        public decimal PARTL_NO { get; set; }
        [Key]
        public DateTime PARTL_HMNIA { get; set; }

        public string PARTL_RECR { get; set; }
        [Key]
        [StringLength(12)]
        public string PSX_PARTL_RECR { get; set; }
        public string PSXETIKA_RECR { get; set; }
        public decimal PSXETIKA_REC0 { get; set; }
        public string SX_COMPANY_CODE { get; set; }
        public string SX_BRANCH_CODE { get; set; }
        public decimal SX_YEAR_YEAR { get; set; }
        public string SX_CTYPKIN_CODE { get; set; }
        public string SX_WTYPKIN_CODE { get; set; }
        public string SX_PTYPPAR_CODE { get; set; }
        public string SX_PSEIRA_SEIRA { get; set; }
        public string SX_CUSTPROMCODE { get; set; }
        public string SX_CLIENT_ID { get; set; }
        public decimal SX_PARTL_NO { get; set; }
        public DateTime SX_PARTL_HMNIA { get; set; }
        public string SX_CTYPKIN_DESC { get; set; }
        public string SX_WTYPKIN_DESC { get; set; }
        public string SX_PTYPPAR_DESC { get; set; }
        public string SX_CUSTPROMDESC { get; set; }
        public string SX_CLIENT_DESC { get; set; }
        public decimal PMS_SX_QTY { get; set; }
    }
}