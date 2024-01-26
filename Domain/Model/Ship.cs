using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("SUBCOMP")]
    public class Ship
    {
        [Column("Code")]
        public decimal Code { get; set; }
        [Column("NAME")]
        public string Name { get; set; }
        [Column("SUM_PCT_TPCL")]
        public decimal? SUM_PCT_TPCL { get; set; }

        [Column("VAT")]
        public string Vat { get; set; }
        [Column("ZIP")]
        public string ZipCode { get; set; }

        public string CountryCodeISO { get; set; }
        [Column("COUNTRY")]
        public string Country { get; set; }

        [Column("MAINCOMP")]
        public decimal Client { get; set; }
    }
}
