using System;

namespace Domain.DTO
{
    public class PmoveDTO
    {
        public decimal Recnum { get; set; }

        public string Company { get; set; }

        public string Branch { get; set; }

        public decimal Year { get; set; }

        public string CUSTPROM_CODE { get; set; }

        public string ClientId { get; set; }

        public string CTYPKIN_CODE { get; set; }

        public string WTYPKIN_CODE { get; set; }

        public string ConstructionCode { get; set; }

        public string PTYPPAR_CODE { get; set; }

        public DateTime Date { get; set; }

        public string Series { get; set; }

        public decimal Number { get; set; }

        public decimal PMS_REC0 { get; set; }

        public string PARTL_RECR { get; set; }

        public decimal? PMS_VATPCT { get; set; }
        public decimal PMS_AMAFTDISC { get; set; }

        public decimal PMS_VATAM { get; set; }

        public decimal POSO_PARAKRAT { get; set; }

        public string AADE_CODE_PARAK { get; set; }

        public decimal POSO_XARTOSH { get; set; }

        public string AADE_CODE_XARTO { get; set; }

        public decimal PMS_DISCAM { get; set; }

        public string ITEM_CODE { get; set; }

        public virtual ItemDTO Item { get; set; }
    }
}