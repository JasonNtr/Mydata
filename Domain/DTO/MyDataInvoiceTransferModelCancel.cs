using System.Collections.Generic;

namespace Domain.DTO
{
    public class MyDataInvoiceTransferModelCancel
    {
        public List<MyDataCancelInvoiceDTO> MyDataInvoices { get; set; } = new List<MyDataCancelInvoiceDTO>();
    }
}