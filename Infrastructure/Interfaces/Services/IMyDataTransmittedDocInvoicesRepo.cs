using Domain.DTO;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.Services
{
    public interface IMyDataTransmittedDocInvoicesRepo
    {
        Task<int> Insert(MyDataTransmittedDocInvoiceDTO transmittedDocDTO);
    }
}
