using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    [Table("INVTYPE")]
    public class InvoiceType
    {
        [Column("CODE")]
        public decimal Code { get; set; }

        [Column("EID_PARAST")]
        public string? EidParast { get; set; }

        [Column("APALLAGH_FPA")]
        
      
        public decimal? VatExemption { get; set; }

        [Column("PISTOTIKO")]
        public decimal? Pistotiko { get; set; }

        [Column("ENHM_MYDATA")]
       
        public decimal? UpdateMyData { get; set; }

        [Column("EXAIRFPA")]
        public decimal? EXAIRFPA { get; set; }

        [Column("SEIRA")]
       
        public string Series { get; set; }
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        public string TYPOS_XARAKTHR { get; set; }

        [Column("SUB")]
        [StringLength(20)]
        public string IsForCancellation { get; set; }

        [Column("EINAI_DELTIO_APOSTOLHS")]
        public byte? IsVoucher { get; set; }

    }
}
