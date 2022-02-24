

using Domain.DTO;

namespace Infrastructure.Database.RequestDocModels
{
    public class InvoiceSummaryType : MyDataEntityDTO
    {
        public virtual double totalNetValue { get; set; }
        public virtual double totalVatAmount { get; set; }
        public virtual double totalWithheldAmounr { get; set; }
        public virtual double totalFeesAmount { get; set; }
        public virtual double totalStumpDutyAmount { get; set; }
        public virtual double totalOtherTaxesAmount { get; set; }
        public virtual double totalDeductionsAmount { get; set; }
        public virtual double totalGrossValue { get; set; }
        //income/expenses Classification types
    }
}
