using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("Companies")]
    public class Company
    {
        [StringLength(10)]
        [Column("Company_Id")]
        public string? CompanyId { get; set; }

        [StringLength(100)]
        [Column("Company_Name")]
        public string? Name { get; set; }

        [StringLength(150)]
        [Column("Company_Address")]
        public string? Address { get; set; }

        [Column("Company_City")]
        public int? CityCode { get; set; }

        [StringLength(20)]
        [Column("Company_Zipcode")]
        public string? Zipcode { get; set; }

        [StringLength(9)]
        [Column("Company_AFM")]
        public string? VatNumber { get; set; }

        [StringLength(30)]
        [Column("Company_DOY")]
        public string? TaxOfficeCode { get; set; }

        [StringLength(50)]
        [Column("Company_Job")]
        public string? Job { get; set; }

        [StringLength(50)]
        [Column("Company_Phone")]
        public string? Phone { get; set; }

        [StringLength(50)]
        [Column("Company_Fax")]
        public string? Fax { get; set; }

        [Column("Company_Temp")]
        public double? Temperature { get; set; }
    }
}