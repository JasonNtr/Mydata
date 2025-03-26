using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("SKOPDIAK")]

    public class MovePurpose
    {
        [Key]
        public decimal CODE { get; set; }
        public string DESCRIPTION { get; set; }
    }
}
