using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("BRANCH")]
    public  class Branch
    {
        [Column("BRANCH_CODE")]
        [StringLength(3)]
        public string Code { get; set; }

        [Column("BRANCH_AFM")]
        [StringLength(20)]
        public string VatNumber { get; set; }

        [Column("BRANCH_ADDR")]
        [StringLength(100)]
        public string Address { get; set; }

        [Column("BRANCH_TK")]
        [StringLength(10)]
        public string ZipCode { get; set; }


        [Column("BRANCH_POLI")]
        [StringLength(30)]
        public string CityName { get; set; }
    }
}
