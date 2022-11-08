using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ClientDTO : EntityDTO
    {
        public string CompanyCode { get; set; }
        public string BranchCode { get; set; }

        public decimal Year { get; set; }

        public string CustomPromCode { get; set; }

        
        public string Name { get; set; }
        public string VatNumber { get; set; }

         
        public string ClientId { get; set; }


        public decimal? CLIENT_REC0 { get; set; }

        public string CUSTPROM_RECR { get; set; }

        public string CLIENT_RECR { get; set; }

        public DateTime? LAST_XREOSH { get; set; }

        public DateTime? LAST_PISTOSH { get; set; }

        public DateTime? LAST_ORDER { get; set; }
        public string CLIENT_DOY { get; set; }

        public string CLIENT_ADDRESS { get; set; }

        public decimal? CLIENT_CITY_ID { get; set; }

        public int? CLIENT_AREA { get; set; }

        public string CLIENT_JOB { get; set; }

        public string CLIENT_RESPONS { get; set; }

        public string CLIENT_ZIPCODE { get; set; }

        public string CLIENT_PHONE { get; set; }

        public string Locked { get; set; }


        public string CLIENT_FAX { get; set; }

        public string CLIENT_MOBILE { get; set; }

        public string CLIENT_AOH { get; set; }

        public string CLIENT_EMAIL { get; set; }

        public string CLIENT_CLASS { get; set; }

        public string CLIENT_DETAILS { get; set; }

        public string CLIENT_SITEW { get; set; }

        public string CLIENT_CODEM { get; set; }

        public decimal? CLIENT_CATEGFPA { get; set; }

        public decimal? CLIENT_FPA { get; set; }

        public decimal? CLIENT_PUBLIC { get; set; }

        public decimal? CLIENT_SN { get; set; }

        public string CLIENT_REPM { get; set; }

        public string CLIENT_ACTIVE { get; set; }

        public string CLIENT_REPORTED { get; set; }

        public string CLIENT_PELPROM { get; set; }

        public DateTime? CLIENT_LASTUPD { get; set; }

        public string CLIENT_MARKEXP { get; set; }

        public decimal? CLIENT_CATJOB { get; set; }

        public string IsSuplier { get; set; }

        public decimal? XREOSH { get; set; }

        public decimal? PISTOSH { get; set; }

        public decimal? YPOLOIPO { get; set; }

        public decimal? PLAFON { get; set; }

        public decimal OVERHEAD { get; set; }

        public string CLIENT_COMPANY { get; set; }

        public string CLIENT_REPMATCH { get; set; }

        public DateTime? CLIENT_HMNIA { get; set; }

        public string CLIENT_PRESP { get; set; }

        public decimal? SYNOLIKH_OFEILH { get; set; }

        public string CLIENT_IDIOT { get; set; }

        public string CLIENT_ADD_NO { get; set; }

        public string CLIENT_NOMOS { get; set; }

        public string CLIENT_XORA { get; set; }

        public string POLH_RECR { get; set; }

        public string PRESP_ID { get; set; }

        public decimal? SUM_PCT_TPCL { get; set; }

        public string IS_CLIENT { get; set; }

        public string SYSXETISH { get; set; }

        public decimal? POSO_SE_APAIT { get; set; }

        public string CLIENT_GLCODE { get; set; }

        public string IS_EKSOTER { get; set; }

        public string IS_XONDR { get; set; }

        public decimal? ORIO_SYN_OFEILH { get; set; }

        public decimal? POSA_APO_DAP { get; set; }

        public string CLIENT_DOYBTM { get; set; }

        public decimal? POSA_APO_PARAG { get; set; }

        public string CLIENT_PTYPPAR { get; set; }

        public string CLIENT_SEIRA { get; set; }

        public decimal? HPSO { get; set; }

        public decimal? HPAL { get; set; }

        public string FEREGIOS { get; set; }

        public decimal? DIAKIN { get; set; }

        public CityDTO City { get; set; }
 

        public override string ToString()
        {
            return Name + " & " + ClientId;
        }
    }
}
