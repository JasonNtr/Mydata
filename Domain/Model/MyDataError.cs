using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataError : MyDataEntity
    {
        [ForeignKey("MyDataResponseId")]
        public virtual Guid MyDataResponseId { get; set; }
        public virtual MyDataResponse MyDataResponse { get; set; }
        public virtual string Message { get; set; }
        public virtual int Code { get; set; }
    }
}
