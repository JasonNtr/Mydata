namespace Domain.DTO
{
    public class ItemDTO
    {
        public string ITEM_CODE { get; set; }

        public string Category { get; set; }

        public string ITEM_DESCR { get; set; }
        public decimal Vat { get; set; }
        public decimal MeasurementUnitCode { get; set; }

        public virtual FpaDTO FPA { get; set; }

    }
}
