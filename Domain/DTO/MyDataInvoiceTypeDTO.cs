using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataInvoiceTypeDTO
    {
        public virtual int Code { get; set; }
        public virtual string Title { get; set; }
        public virtual string ShortTitle { get; set; }
        public virtual string Description { get; set; }
        public virtual char sign { get; set; }
    }
}
