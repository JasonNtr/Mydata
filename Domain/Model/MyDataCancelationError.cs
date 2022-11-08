using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class MyDataCancelationError : MyDataEntity
    {
        [ForeignKey("MyDataCancelationResponseId")]
        public virtual Guid MyDataCancelationResponseId { get; set; }

        public virtual MyDataCancelationResponse MyDataCancelationResponse { get; set; }
        public virtual string message { get; set; }
        public virtual int code { get; set; }
    }
}