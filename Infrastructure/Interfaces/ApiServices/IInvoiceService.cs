using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces.ApiServices
{
    public interface IInvoiceService
    {
        Task<int> PostAction(string filePath);
        Task<int> PostInvoice(MyDataInvoiceDTO myDataInvoiceDTO, string invoiceFilePath);
        Task<int> CancelInvoice(MyDataInvoiceDTO myDataInvoiceDTO);
        MyDataResponseDTO ParseInvoicePostResult(string httpResponseContext);
        Task<MyDataInvoiceDTO> BuildInvoice(string filenamePath);
        Task<int> RequestDocs(string mark);
        Task<int> CancelInvoiceBatchProcess(string uid);
    }
}
