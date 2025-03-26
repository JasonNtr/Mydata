using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ClientDTO
    {
        public decimal Code { get; set; }
        public string Name { get; set; }

        public decimal? SUM_PCT_TPCL { get; set; }

        public string Vat { get; set; }
         public string ZipCode { get; set; }

        public string CountryCodeISO { get; set; }
         public string Country { get; set; }

         public string City { get; set; }
        public string Address { get; set; }
        public virtual ShipDTO Ship{ get; set; }

    }
}
