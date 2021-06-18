using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces.ApiServices
{
    public interface IExpenseService
    {
        Task<int> PostAction(string filePath);
        Task<int> PostInvoice(MyDataInvoiceDTO myDataInvoiceDTO, string invoiceFilePath);
        Task<int> CancelInvoice(MyDataInvoiceDTO myDataInvoiceDTO);
        MyDataResponseDTO ParseInvoicePostResult(string httpResponseContext);
        Task<MyDataInvoiceDTO> BuildInvoice(string filenamePath);
    }
}
