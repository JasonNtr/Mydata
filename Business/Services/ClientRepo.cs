using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ClientRepo : Repo
    {
        public ClientRepo(string connectionString) : base(connectionString)
        {
        }

        public async Task<bool> UpdateSpecific()
        {
            var context = GetContext();
            var clients = await context.Clients.Where(x => x.CountryCodeISO == null || x.SUM_PCT_TPCL == null).ToListAsync();
            foreach (var client in clients)
            {
                client.CountryCodeISO = "GR";
                client.SUM_PCT_TPCL = 24;
            }
            var result = await context.SaveChangesAsync();
            return result > 0;
        }
    }
}