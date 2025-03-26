using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ItemRepo : Repo
    {
        public ItemRepo(string connectionString) : base(connectionString)
        {
        }

        public async Task<List<string>> GetCategories()
        {
            var context = GetContext();
            var categories = await context.Pmoves.Select(x => x.ItemCategory).Where(x => x != null).Distinct().OrderBy(x => x).ToListAsync();
            return categories;
        }
    }
}