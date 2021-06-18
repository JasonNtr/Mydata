using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces.Services
{
    public interface IMyDataIncomeResponseRepo
    {
        Task<int> Insert(MyDataIncomeResponseDTO myDataIncomeResponseDTO);
    }
}
