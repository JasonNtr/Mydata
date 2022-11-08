using System.Collections.Generic;

namespace Domain.DTO
{
    public  class MyDataInvoiceTransferModel
    {
        public List<MyDataInvoiceDTO> MyDataInvoices { get; set; } = new List<MyDataInvoiceDTO>();
        public string Xml { get; set; }
        
    }
}
