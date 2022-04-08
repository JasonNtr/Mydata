using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Model
{
    public class MyDataInvoiceHeaderType : MyDataEntity
    {
        [ForeignKey("MyDataDocInvoiceId")]
        public virtual MyDataTransmittedDocInvoice MyDataDocInvoice { get; set; }
        public virtual Guid MyDataDocInvoiceId { get; set; }
        [StringLength(50)]
        public virtual string series { get; set; }
        [StringLength(50)]
        public virtual string aa { get; set; }
        public virtual DateTime issueDate { get; set; }
        [StringLength(20)]
        public virtual string invoiceType { get; set; }
        public virtual bool? vatPaymentSuspension { get; set; }
        [StringLength(20)]
        public virtual string currency { get; set; }


        //Not Used
        public virtual double? exchangeRate { get; set; }
        public virtual long? correlatedInvoices { get; set; }
        public virtual bool? selfPricing { get; set; }
        public virtual DateTime? dispatchDate { get; set; }
        [StringLength(50)]
        public virtual string dispatchTime { get; set; }
        [StringLength(50)]
        public virtual string vehicleNumber { get; set; }
        public virtual int? movePurpose { get; set; }

    }
}
