using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Domain.DTO;

namespace Infrastructure.Interfaces.ApiServices
{
    public interface IInvoiceService
    {
        Task<int> PostAction(string file_path);
        Task<int> PostInvoice(MyDataInvoiceDTO mydatainvoicedto, string invoice_file_path);
        Task<int> CancelInvoice(MyDataInvoiceDTO mydatainvoicedto);




        MyDataResponseDTO ParseInvoicePostResult(string httpresponsecontext);
        Task<MyDataInvoiceDTO> BuildInvoice(string filename);
        
    }
}
