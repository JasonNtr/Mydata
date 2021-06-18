using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces.Services
{
    public interface IIncomeRepo
    {
        Task<List<MyDataIncomeDTO>> GetList(DateTime fromDate, DateTime ToDate);
        Task<MyDataIncomeDTO> Insert(MyDataIncomeDTO myDataIncomeDTO);
        Task<MyDataIncomeDTO> Update(MyDataIncomeDTO myDataIncomeDTO);
        Task<bool> ExistedUid(long? Uid);

        Task<MyDataIncomeDTO> GetByUid(long? Uid);
    }
}
