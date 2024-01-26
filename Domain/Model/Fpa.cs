using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("FPA")]
    public class Fpa
    {
        [Column("FPA_POSOSTO")]

        public decimal Percentage { get; set; }
        [Column("KATHGFPA")]

        public decimal Category { get; set; }
    }
}
