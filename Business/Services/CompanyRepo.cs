using Domain.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CompanyRepo : Repo
    {
        public CompanyRepo(string connectionString) : base(connectionString)
        {

        }

        public async Task<CompanyDTO> Get()
        {
            var context = GetContext();

            try
            {
                var company2 = await context.Companies.AsNoTracking().FirstOrDefaultAsync();
            }
            catch (Exception x )
            {

                throw;
            }
            var company = await context.Companies.AsNoTracking().FirstOrDefaultAsync();
            var companyDTO = Mapper.Map<CompanyDTO>(company);
            return companyDTO;
        }
    }
}
