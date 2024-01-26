using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("PARFOROL")]
    public class TaxInvoice
    {
        [Key]
        [Column("KODIKOS_FOROL")]
        [StringLength(3)]
        public decimal TaxCode { get; set; }

        [Key]
        [Column("PTYPPAR_CODE")]
        [StringLength(3)]
        public string PtyparCode { get; set; }

        [Key]
        [Column("MODULE")]
        [StringLength(2)]
        public string Module { get; set; }

        [Column("PTYPPAR_DESCR")]
        [StringLength(60)]
        public string PtyparDescription { get; set; }

        [Column("SIGN")]
        [StringLength(1)]
        public string Sign { get; set; }

        [Column("PERIGRAFH_FOROL")]
        [StringLength(100)]
        public string TaxDescription { get; set; }

        [StringLength(15)]
        public string PERIG_FOROL_SYN { get; set; }
    }
}
