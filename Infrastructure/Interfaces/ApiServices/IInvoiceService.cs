﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces.ApiServices
{
    public interface IInvoiceService
    {
        Task<int> PostAction(string filePath);
        //Task<int> PostInvoice(MyDataInvoiceDTO myDataInvoiceDTO, string invoiceFilePath);
        Task<int> CancelInvoice(MyDataInvoiceDTO myDataInvoiceDTO);
        List<MyDataResponseDTO> ParseInvoicePostResult(string httpResponseContext);
        Task<MyDataInvoiceDTO> BuildInvoice(string filenamePath);
        Task<int> RequestDocs(string mark);
        Task<int> CancelInvoiceBatchProcess(MyDataInvoiceDTO mydataInvoiceDTO);
        Task<MyDataInvoiceDTO> BuildInvoiceBatchProcess(MyDataInvoiceDTO mydataInvoiceDTO);
        bool CreateLogFileForBatchProcess(string selectedPath = "");
    }
}
