using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
    [Table("CLIENTS")]
    public class Client : Entity
    {

        [Key]
        [Column("COMPANY_CODE", Order = 1)]
        [StringLength(3)]
        public string CompanyCode { get; set; }

        [Key]
        [Column("BRANCH_CODE", Order = 2)]
        [StringLength(3)]
        public string BranchCode { get; set; }

        [Key]
        [Column("YEAR_YEAR", Order = 3, TypeName = "numeric")]
        public decimal Year { get; set; }

        [Key]
        [Column("CUSTPROM_CODE", Order = 4)]
        [StringLength(20)]
        public string CustomPromCode { get; set; }

        [Key]
        [Column("CLIENT_ID", Order = 5)]
        [StringLength(20)]
        public string ClientId { get; set; }

        [Column("CLIENT_NAME")]
        [StringLength(105)]
        public string Name { get; set; }

        //[Index(IsUnique = true)]
        [Column("CLIENT_AFM")]
        [StringLength(20)]
        public string VatNumber { get; set; }

        public decimal? CLIENT_REC0 { get; set; }
        [StringLength(12)]
        public string CUSTPROM_RECR { get; set; }
        [StringLength(12)]
        public string CLIENT_RECR { get; set; }

        public DateTime? LAST_XREOSH { get; set; }

        public DateTime? LAST_PISTOSH { get; set; }

        public DateTime? LAST_ORDER { get; set; }

        [StringLength(5)]
        public string CLIENT_DOY { get; set; }

        [StringLength(250)]
        public string CLIENT_ADDRESS { get; set; }

        public decimal? CLIENT_CITY_ID { get; set; }

        public int? CLIENT_AREA { get; set; }

        [StringLength(250)]
        public string CLIENT_JOB { get; set; }

        [StringLength(80)]
        public string CLIENT_RESPONS { get; set; }

        [StringLength(50)]
        public string CLIENT_ZIPCODE { get; set; }

        [StringLength(100)]
        public string CLIENT_PHONE { get; set; }
        [StringLength(30)]
        public string CLIENT_FAX { get; set; }
        [StringLength(30)]
        public string CLIENT_MOBILE { get; set; }
        [StringLength(50)]
        public string CLIENT_AOH { get; set; }
        [StringLength(80)]
        public string CLIENT_EMAIL { get; set; }
        [StringLength(5)]
        public string CLIENT_CLASS { get; set; }

        public string CLIENT_DETAILS { get; set; }

        [StringLength(10)]
        public string CLIENT_SITEW { get; set; }

        [StringLength(20)]
        public string CLIENT_CODEM { get; set; }

        public decimal? CLIENT_CATEGFPA { get; set; }

        public decimal? CLIENT_FPA { get; set; }

        public decimal? CLIENT_PUBLIC { get; set; }

        public decimal? CLIENT_SN { get; set; }

        [StringLength(20)]
        public string CLIENT_REPM { get; set; }

        [StringLength(1)]
        public string CLIENT_ACTIVE { get; set; }

        [StringLength(1)]
        public string CLIENT_REPORTED { get; set; }

        [StringLength(1)]
        public string CLIENT_PELPROM { get; set; }

        public DateTime? CLIENT_LASTUPD { get; set; }

        [StringLength(1)]
        public string CLIENT_MARKEXP { get; set; }

        public decimal? CLIENT_CATJOB { get; set; }

        [StringLength(1)]
        [Column("IS_PROMHTHEYTHS")]
        public string IsSuplier { get; set; }

        public decimal? XREOSH { get; set; }

        public decimal? PISTOSH { get; set; }

        public decimal? YPOLOIPO { get; set; }

        public decimal? PLAFON { get; set; }

        public decimal? OVERHEAD { get; set; }

        [StringLength(3)]
        public string CLIENT_COMPANY { get; set; }
        [StringLength(20)]
        public string CLIENT_REPMATCH { get; set; }

        public DateTime? CLIENT_HMNIA { get; set; }
        [StringLength(20)]
        public string CLIENT_PRESP { get; set; }

        [Column("CLIENT_LOCKED")]
        [StringLength(105)]
        public string Locked { get; set; }

        public decimal? SYNOLIKH_OFEILH { get; set; }

        [StringLength(2)]
        public string CLIENT_IDIOT { get; set; }

        [StringLength(10)]
        public string CLIENT_ADD_NO { get; set; }

        [StringLength(5)]
        public string CLIENT_NOMOS { get; set; }

        [StringLength(5)]
        public string CLIENT_XORA { get; set; }

        [StringLength(12)]
        public string POLH_RECR { get; set; }

        [StringLength(20)]
        public string PRESP_ID { get; set; }

        public decimal? SUM_PCT_TPCL { get; set; }

        [StringLength(1)]
        public string IS_CLIENT { get; set; }

        [StringLength(20)]
        public string SYSXETISH { get; set; }

        public decimal? POSO_SE_APAIT { get; set; }

        [StringLength(30)]
        public string CLIENT_GLCODE { get; set; }

        [StringLength(1)]
        public string IS_EKSOTER { get; set; }

        [StringLength(1)]
        public string IS_XONDR { get; set; }

        public decimal? ORIO_SYN_OFEILH { get; set; }

        public decimal? POSA_APO_DAP { get; set; }
        [StringLength(20)]
        public string CLIENT_DOYBTM { get; set; }

        public decimal? POSA_APO_PARAG { get; set; }
        [StringLength(5)]
        public string CLIENT_PTYPPAR { get; set; }
        [StringLength(5)]
        public string CLIENT_SEIRA { get; set; }

        public decimal? HPSO { get; set; }

        public decimal? HPAL { get; set; }
        [StringLength(2)]
        public string FEREGIOS { get; set; }

        public decimal? DIAKIN { get; set; }
        [StringLength(1)]
        public string STATUS { get; set; }
        [StringLength(1)]
        public string ANAL { get; set; }
        [StringLength(1)]
        public string ENDOOMIL { get; set; }
        [StringLength(1)]
        public string NPDD { get; set; }
    }
}
