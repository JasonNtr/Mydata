﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Infrastructure.Interfaces.Services
{
    public interface IExpenseRepo
    {
        Task<int> AddIfNotExist(MyDataInvoiceDTO mydatainvoicedto);
        Task<MyDataInvoiceDTO> Insert(MyDataInvoiceDTO mydatainvoicedto);
        Task<MyDataInvoiceDTO> Update(MyDataInvoiceDTO mydatainvoicedto);
        Task<int> AddResponses(MyDataInvoiceDTO mydatainvoicedto);
        Task<bool> ExistedUid(long? Uid);
        Task<MyDataInvoiceDTO> Get();
        Task<MyDataInvoiceDTO> GetByUid(long? Uid);
        Task<List<MyDataInvoiceDTO>> GetList();
        Task<List<MyDataInvoiceDTO>> GetList(DateTime fromDate, DateTime ToDate);
        Task<List<MyDataInvoiceDTO>> GetList(DateTime selectedDate);
        Task<MyDataInvoiceDTO> GetByMark(long invoiceMark);
    }
}
