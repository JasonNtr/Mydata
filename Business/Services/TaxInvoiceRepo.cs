using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class TaxInvoiceRepo : Repo
    {

        public TaxInvoiceRepo(string connectionString) : base(connectionString)
        {

        }

        public async Task<decimal?> GetTaxCode(string ptyparCode)
        {
            var context = GetContext();
            var tax = await context.TaxInvoices.FirstOrDefaultAsync(x => x.PtyparCode.Equals(ptyparCode));
            return tax?.TaxCode;
        }
    }
}