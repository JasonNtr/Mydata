using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("INVDTL")]

    public class PMove
    {
        [Column("CODE")]
        public decimal Code { get; set; }

        [Column("INVHDR")]
        public decimal Particle { get; set; }

        [Column("PMS_AMAFTDISC")]
        public decimal? PMS_AMAFTDISC { get; set; }
        [Column("POSO_XARTOSH")]
        public decimal? POSO_XARTOSH { get; set; }
        [Column("PMS_VATAM")]
        public decimal? PMS_VATAM { get; set; }
        [Column("PMS_DISCAM")]
        public decimal? PMS_DISCAM { get; set; }

        [Column("PMS_VATPCT")]
    
        public decimal? PMS_VATPCT { get; set;}

        [Column("ITEM_CODE")]
        public string? Item { get; set; }

        [Column("PMS_QTY")]
        public decimal? Quantity { get; set; }

        [Column("PMS_UNITPRICE")]
        public decimal? UnitPrice { get; set; }

        [Column("POSOSTO_XARTOSH")]
        public decimal? POSOSTO_XARTOSH { get; set; }

        [Column("PMS_UNITCODE")]
        public string MeasurementUnitCode { get; set; }

        [Column("otherMeasurementUnitQuantity")]
        public decimal? OtherMeasurementUnitQuantity { get; set; }

        [Column("ItemCategory")]
        [StringLength(20)]
        public string ItemCategory { get; set; }

        [Column("AADE_CODE_PARAK")]
        [StringLength(20)]
        public string AADE_CODE_PARAK { get; set; }
    }
}
