namespace Domain.DTO
{
    public class CityDTO : EntityDTO
    {

        public decimal CityId { get; set; }

        
        public int IntegerCityId => decimal.ToInt32(CityId);
 
        public string Name { get; set; }
        public decimal? TOWN_ID { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}