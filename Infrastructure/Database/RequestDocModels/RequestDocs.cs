using Domain.DTO;
using System.Collections.Generic;

namespace Infrastructure.Database.RequestDocModels
{
    public class RequestDocs
    {
        public invoicesDoc invoicesDoc = new invoicesDoc();
    }

    public class invoicesDoc
    {
        public List<MyDataTransmittedDocInvoiceDTO> invoice = new List<MyDataTransmittedDocInvoiceDTO>();
    }
}
