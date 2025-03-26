using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
    [Table("EIDOSPOS")]

    public class MeasurementUnit
    {
        [Key]
        public decimal CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string AME_UNIT_CODE { get; set; }

        public string AME_UNIT_DESCR { get; set; }

    }
}
