namespace Domain.DTO
{
    public class IncomeClassificationForEditInvoice
    {
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }
        public Enums.Enums.IncomeClassificationValueType CharacterizationType { get; set; }
        public Enums.Enums.IncomeClassificationCategoryType CharacterizationCategory { get; set; }
        public decimal Amount { get; set; }
    }
}