namespace Domain.DTO
{
    public class ShipDTO
    {
        public decimal Code { get; set; }
         public string Name { get; set; }
         public decimal? SUM_PCT_TPCL { get; set; }

         public string Vat { get; set; }
         public string ZipCode { get; set; }

        public string CountryCodeISO { get; set; }
         public string Country { get; set; }

         public decimal Client { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }

    }
}
