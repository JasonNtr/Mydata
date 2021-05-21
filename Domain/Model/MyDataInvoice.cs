﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataInvoice : MyDataEntity
    {
        public virtual long? Uid { get; set; }
        public virtual DateTime? InvoiceDate { get; set; }
        public virtual long? InvoiceNumber { get; set; }
        public virtual string VAT { get; set; }
        public virtual int InvoiceTypeCode { get; set; }
        public virtual MyDataInvoiceType InvoiceType { get; set; }
        public virtual string FileName { get; set; }
        public virtual string StoredXml { get; set; }
        public virtual ICollection<MyDataResponse> MyDataResponses { get; set; } = new List<MyDataResponse>();


        public virtual long? CancellationMark { get; set; }
        public virtual ICollection<MyDataCancelationResponse> MyDataCancelationResponses { get; set; } = new List<MyDataCancelationResponse>();
    }
}
