using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Model
{
    [Table("CITIES")]
    public class City : Entity
    {
        [Key, Column("CITY_ID")]
        public decimal CityId { get; set; }

        [Required]
        [StringLength(30)]
        [Column("CITY_DESCR")]
        public string Name { get; set; }

        public decimal? TOWN_ID { get; set; }

    }
}

