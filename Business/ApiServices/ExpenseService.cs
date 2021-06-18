using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.ApiServices;
using Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Business.ApiServices
{
    public class ExpenseService : IExpenseService
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly IInvoiceRepo _invoiceRepo;
        private readonly IMyDataResponseRepo _myDataResponseRepo;
        private readonly IMyDataCancellationResponseRepo _myDataCancellationResponseRepo;
        private readonly IParticleInform _particleInform;
        public Task<int> PostAction(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task<int> PostInvoice(MyDataInvoiceDTO myDataInvoiceDTO, string invoiceFilePath)
        {
            throw new NotImplementedException();
        }

        public Task<int> CancelInvoice(MyDataInvoiceDTO myDataInvoiceDTO)
        {
            throw new NotImplementedException();
        }

        public MyDataResponseDTO ParseInvoicePostResult(string httpResponseContext)
        {
            throw new NotImplementedException();
        }

        public Task<MyDataInvoiceDTO> BuildInvoice(string filenamePath)
        {
            throw new NotImplementedException();
        }
    }
}
