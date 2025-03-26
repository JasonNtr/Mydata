using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("COMPANY")]
    public class Company
    {
        [Column("COMPANY_CODE")]
        public string Code { get; set; }
        [Column("COMPANY_NAME")]
        public string Name { get; set; }
            [Column("COMPANY_COUNTRY")]
           
            public string Country { get; set; }
            [Column("COMPANY_AFM")]
        
            public string Vat { get; set; }
        [Column("COMPANY_TK")]
        public string ZipCode { get; set; }

        [Column("COMPANY_CITY")]
        public string CIty { get; set; }

        [Column("COMPANY_ADDR")]
        public string Address { get; set; }

    }
}
