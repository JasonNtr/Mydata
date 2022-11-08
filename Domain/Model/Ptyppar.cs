using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("PTYPPAR")]
    public class Ptyppar
    {
        [Column("PTYPPAR_RECR")]
        [StringLength(12)]
        public string PTYPPAR_RECR { get; set; }

        [Column("APALLAGH_FPA")]
        public decimal? APALLAGH_FPA { get; set; }


        [Column("EXAIRFPA")]
        public decimal? EXAIRFPA { get; set; }

        [Column("EID_PARAST")]
        [StringLength(10)]
        public string EID_PARAST { get; set; }

        [Column("SUB")]
        [StringLength(20)]
        public string IsForCancellation { get; set; }

        [Column("PISTOTIKO")]
        public decimal? PISTOTIKO { get; set; }

        [Column("TYPOS_XARAKTHR")]
        [StringLength(20)]
        public string TYPOS_XARAKTHR { get; set; }

        [Column("PARAKFOR")]
        public decimal? PARAKFOR { get; set; }

        [Column("SYNTXART")]
        public decimal? SYNTXART { get; set; }

        [Column("ENHM_MYDATA")]
        public decimal UpdateMyData { get; set; }

        [Column("PTYPPAR_DESCR")]
        public string Description { get; set; }
        [Column("PTYPPAR_CODE")]
        public string Code { get; set; }

    }
}
