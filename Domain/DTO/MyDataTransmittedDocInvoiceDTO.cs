using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataTransmittedDocInvoiceDTO : MyDataEntityDTO
    {
        //[JsonProperty("uid")]
        public virtual string? Uid { get; set; }
        public virtual string? authenticationCode { get; set; }
        public virtual long? mark { get; set; }


        public virtual ICollection<MyDataPartyTypeDTO> issuer { get; set; } = new List<MyDataPartyTypeDTO>();
        public virtual ICollection<MyDataPartyTypeDTO> counterpart { get; set; } = new List<MyDataPartyTypeDTO>();


        public virtual MyDataInvoiceHeaderTypeDTO invoiceHeaderType { get; set; }

        //[JsonArray("paymentMethods", typeof(MyDataPaymentMethodDetailDTO))]
        public virtual ICollection<MyDataPaymentMethodDetailDTO> paymentMethodDetailType { get; set; } = new List<MyDataPaymentMethodDetailDTO>();
        public virtual ICollection<MyDataInvoiceRowTypeDTO> invoiceDetails { get; set; } = new List<MyDataInvoiceRowTypeDTO>();
        public virtual ICollection<MyDataTaxesDTO> taxesTotals { get; set; } = new List<MyDataTaxesDTO>();
        public virtual MyDataInvoiceSummaryDTO invoiceSummary { get; set; }
    }
}
