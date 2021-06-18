using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Domain.Model;
using Infrastructure.Database;
using Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class IncomeRepo : IIncomeRepo
    {
        private readonly ApplicationDbContext ctx;
        private readonly IMapper mapper;

        public IncomeRepo(ApplicationDbContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }


        public async Task<MyDataIncomeDTO> Insert(MyDataIncomeDTO myDataIncomeDTO)
        {
            var newMyDataIncome = new MyDataIncome();
            mapper.Map(myDataIncomeDTO, newMyDataIncome);
            await ctx.MyDataIncomes.AddAsync(newMyDataIncome);
            var result = await ctx.SaveChangesAsync();
            var myDataInvoiceDTORefreshed = await GetByUid(newMyDataIncome.Uid);
            return myDataInvoiceDTORefreshed;
        }

        public async Task<MyDataIncomeDTO> Update(MyDataIncomeDTO myDataIncomeDTO)
        {
           var newMyDataIncome = await ctx.MyDataIncomes
                .FirstOrDefaultAsync(x => x.Uid == myDataIncomeDTO.Uid);
            mapper.Map(myDataIncomeDTO, newMyDataIncome);
            ctx.Update(newMyDataIncome);
            var result = await ctx.SaveChangesAsync();
            var myDataInvoiceDTORefreshed = await GetByUid(newMyDataIncome.Uid);
            return myDataInvoiceDTORefreshed;
        }


        public async Task<bool> ExistedUid(long? Uid)
        {
            if (Uid == null) 
                return false;

            var exist =
                await ctx.MyDataIncomes
                    .AnyAsync(x => x.Uid == Uid);
            return exist;
        }

        public async Task<MyDataIncomeDTO> GetByUid(long? Uid)
        {
            var myDataIncome =
                await ctx.MyDataIncomes
                    .FirstOrDefaultAsync(x => x.Uid == Uid);
            var myDataIncomeDTO = mapper.Map<MyDataIncomeDTO>(myDataIncome);
            return myDataIncomeDTO;
        }

        public async Task<List<MyDataIncomeDTO>> GetList(DateTime fromDate, DateTime toDate)
        {

            try
            {
                var list =
                    await ctx.MyDataIncomes
                        .Where(x => x.Modified.Date >= fromDate.Date && x.Modified.Date <= toDate.Date)
                        .Include(p => p.MyDataIncomeResponses).ThenInclude(p => p.Errors)
                        .OrderByDescending(x => x.Modified)
                        .ToListAsync();

                var myDataIncomeDTO = mapper.Map<List<MyDataIncomeDTO>>(list);
                return myDataIncomeDTO;
            }
            catch (Exception ex)
            {

            }

            return new List<MyDataIncomeDTO>();

        }

    }
}
