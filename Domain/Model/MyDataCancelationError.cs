using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataCancelationError : MyDataEntity
    {
        [ForeignKey("MyDataCancelationResponseId")]
        public virtual Guid MyDataCancelationResponseId { get; set; }
        public virtual MyDataCancelationResponse MyDataCancelationResponse { get; set; }
        public virtual string Message { get; set; }
        public virtual int Code { get; set; }
    }
}
