using System.Collections.Generic;

namespace Domain.DTO
{
    public  class MyDataInvoiceTransferModel
    {
        public List<MyDataInvoiceDTO> MyDataInvoices { get; set; } = new List<MyDataInvoiceDTO>();
        public List<MyDataCancelInvoiceDTO> MyCancelDataInvoices { get; set; } = new List<MyDataCancelInvoiceDTO>();
        public string Xml { get; set; }
        public List<string> XmlPerInvoice { get; set; }
        
    }
}
