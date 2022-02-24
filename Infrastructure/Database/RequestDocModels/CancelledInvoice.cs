using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Infrastructure.Database.RequestDocModels
{
    public class CancelledInvoice : MyDataEntityDTO
    {
        public virtual long? invoiceMark { get; set; }
        public virtual long? cancellationMark { get; set; }
        public virtual DateTime? cancellationDate { get; set; }
    }
}
