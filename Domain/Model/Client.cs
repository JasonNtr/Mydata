using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("MAINCOMP")]

    public class Client
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
        public string Country{ get; set; }
        
        [Column("AREA")]
        public string City{ get; set; }

        [Column("ADDRESS")]
        public string Address { get; set; }
    }
}
