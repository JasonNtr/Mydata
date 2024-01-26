using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("ITEM")]

    public class Item
    {
         
        public string ITEM_CODE { get; set; }

        [Column("KATHG_XARAKTHR")]
       
        public string Category { get; set; }
        public string ITEM_DESCR { get; set; }
        [Column("FPA_POSOSTO")]
        public decimal Vat { get; set; }

    }
}
