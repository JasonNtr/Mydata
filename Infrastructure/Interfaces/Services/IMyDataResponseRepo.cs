using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Model;

namespace Infrastructure.Interfaces.Services
{
    public interface IMyDataResponseRepo
    {
        Task<int> Insert(MyDataResponseDTO mydataresponsedto);
    }
}
