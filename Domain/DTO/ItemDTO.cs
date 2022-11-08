namespace Domain.DTO
{
    public class ItemDTO
    {
        public string UNITS_CODE { get; set; }

        public string UNITS_RECR { get; set; }

        public string WITEMKAT_CODE { get; set; }

        public string WITEMKAT_RECR { get; set; }

        public string ITEM_CODE { get; set; }

        public decimal ITEM_REC0 { get; set; }

        public string ITEM_DESCR { get; set; }

        public string ITEM_RECR { get; set; }

        public decimal FPA_POSOSTO { get; set; }

        public string ITEM_QUAL { get; set; }

        public string WAPOKAT_CODE { get; set; }

        public string ITEM_ANALYSH { get; set; }

        public decimal? ITEM_ORIO { get; set; }

      

        public string CODEGLPOL { get; set; }

        public string CODEGLAGO { get; set; }

        public string LINK1 { get; set; }

        public string LINK2 { get; set; }

        public decimal? GROUP { get; set; }

        public string KATHG_XARAKTHR { get; set; }

        public virtual FPADTO FPA { get; set; }
    }
}