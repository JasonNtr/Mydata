using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces
{
    public interface IParticleInform
    {
        Task UpdateParticle(MyDataInvoiceDTO mydatainvoicedto);
        Task UpdateCancellationParticle(MyDataInvoiceDTO mydatainvoicedto, MyDataInvoiceDTO mydatainvoicedtobecancelled);
    }
}
