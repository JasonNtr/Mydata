using Domain.DTO;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces.Services
{
    public interface IMyDataTransmittedDocInvoicesRepo
    {
        Task<int> Insert(MyDataTransmittedDocInvoiceDTO transmittedDocDTO);
        Task<int> Update(MyDataTransmittedDocInvoiceDTO transmittedDocDTO);
        Task<bool> ExistsMark(long? mark);
        Task<MyDataTransmittedDocInvoiceDTO> GetByMark(long? mark);
    }
}
