
using Domain.DTO;

namespace Infrastructure.Database.RequestDocModels
{
    public class PaymentMethodDetail : MyDataEntityDTO
    {
        public virtual int type { get; set; }
        public virtual double amount { get; set; }
        public virtual string paymentMethodInfo { get; set; }
    }
}
