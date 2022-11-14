namespace Domain.DTO
{
    public class IncomeClassificationForEditInvoice
    {
        public string ItemDescription { get; set; }
        public string CharacterizationType { get; set; }
        public string CharacterizationCategory { get; set; }
        public decimal Amount { get; set; }
    }
}