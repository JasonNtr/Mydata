using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces.ApiServices
{
    public interface IIncomeService
    {
        Task<int> PostIncome(string invoiceFilePath);
        MyDataIncomeResponseDTO ParseIncomePostResult(string httpResponseContext);
        Task<MyDataIncomeDTO> BuildIncome(string filenamePath);
    }
}
