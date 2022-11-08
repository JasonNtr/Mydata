using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
    [Table("FPA")]
    public class FPA 
    {
        [Key]
        [Column("FPA_POSOSTO")]
        public decimal Percentage { get; set; }

        [Column("FPA_DESCR")]
        [StringLength(20)]
        public string Description { get; set; }

        [Column("AGORA_CODEGL")]
        [StringLength(20)]
        public string PurchasesAccountingCode { get; set; }

        [Column("POLHSH_CODEGL")]
        [StringLength(20)]
        public string SalesAccountingCode { get; set; }

        [Column("KATHGFPA")]
        public decimal? FpaCategory { get; set; }


    }
}
