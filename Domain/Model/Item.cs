using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    [Table("ITEM")]
    public class Item
    {
        //public decimal? RECNUM { get; set; }

        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string UNITS_CODE { get; set; }

        [StringLength(12)]
        public string UNITS_RECR { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(20)]
        public string WITEMKAT_CODE { get; set; }

        [StringLength(12)]
        public string WITEMKAT_RECR { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string ITEM_CODE { get; set; }

        public decimal ITEM_REC0 { get; set; }

        [Required]
        [StringLength(150)]
        public string ITEM_DESCR { get; set; }

        [Required]
        [StringLength(12)]
        public string ITEM_RECR { get; set; }

        public decimal FPA_POSOSTO { get; set; }

        [Required]
        [StringLength(10)]
        public string ITEM_QUAL { get; set; }

        [StringLength(10)]
        public string WAPOKAT_CODE { get; set; }

        [StringLength(1)]
        public string ITEM_ANALYSH { get; set; }

        public decimal? ITEM_ORIO { get; set; }

        

        [StringLength(20)]
        public string CODEGLPOL { get; set; }

        [StringLength(20)]
        public string CODEGLAGO { get; set; }

        [StringLength(20)]
        public string LINK1 { get; set; }

        [StringLength(20)]
        public string LINK2 { get; set; }

        public decimal? GROUP { get; set; }

        [StringLength(20)]
        public string KATHG_XARAKTHR { get; set; }

    }
}
