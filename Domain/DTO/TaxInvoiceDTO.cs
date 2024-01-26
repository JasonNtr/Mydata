using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class TaxInvoiceDTO
    {
        public decimal TaxCode { get; set; }

        public string PtyparCode { get; set; }

        public string Module { get; set; }

        public string PtyparDescription { get; set; }

        public string Sign { get; set; }

        public string TaxDescription { get; set; }

        public string PERIG_FOROL_SYN { get; set; }
    }
}
