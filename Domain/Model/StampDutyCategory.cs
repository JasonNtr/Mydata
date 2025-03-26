using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("SYNTXART")]

    public class StampDutyCategory
    {
        [Key]
        [Column("CODE")]
        public decimal Code { get; set; }
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("XARTOSHMO_PCT")]
        public decimal StampDuty { get; set; }

    }
}
