using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyGenericErrorsDTO
    {
        public virtual DateTime Modified { get; set; } = DateTime.Now;
        public virtual string Message { get; set; }
        public virtual int Code { get; set; }
    }
}
