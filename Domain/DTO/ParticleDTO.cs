using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.DTO
{
    public class ParticleDTO
    {
        public decimal Recnum { get; set; }

        public string Company { get; set; }

        public string Branch { get; set; }
        public string PTYPPAR_CODE { get; set; }
        public decimal Year { get; set; }

        public string CTYPKIN_CODE { get; set; }

        public string WTYPKIN_CODE { get; set; }

        public string TypeCode { get; set; }

        public string Series { get; set; }

        public string CUSTPROM_CODE { get; set; }

        public string ClientId { get; set; }

        public decimal Number { get; set; }
        public string Module { get; set; }
        public DateTime Date { get; set; }

        public string PARTL_RECR { get; set; }

        public string PTYPPAR_RECR { get; set; }

        public decimal Paymentmethod { get; set; }

        public decimal TrasnferPurpose { get; set; }

        public string CurrencyCode { get; set; }

        public decimal Amount { get; set; }

        public decimal TotalNetAmount { get; set; }

        public decimal TotalVatAmount { get; set; }

        public decimal TotalOtherTaxesAmount { get; set; }

        public decimal TotalDeductionsAmount { get; set; }

        public decimal WithHeldAmount { get; set; }
        public string Mark { get; set; }
        public string CancelMark { get; set; }
        public string Closed { get; set; }
        public decimal Rec0 { get; set; }

        public string Printed { get; set; }

        public virtual PtypparDTO Ptyppar { get; set; }
        public virtual List<PmoveDTO> Pmoves { get; set; }
        public virtual ClientDTO Client { get; set; }
        public virtual BranchDTO BranchDTO { get; set; }

        public Guid DataGridId { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ParticleDTO other))
                return false;

            return Company.Equals(other.Company) &&
                   Branch.Equals(other.Branch) &&
                   CTYPKIN_CODE.Equals(other.CTYPKIN_CODE) &&
                   WTYPKIN_CODE.Equals(other.WTYPKIN_CODE) &&
                   PTYPPAR_CODE.Equals(other.PTYPPAR_CODE) &&
                   Series.Equals(other.Series) &&
                   CUSTPROM_CODE.Equals(other.CUSTPROM_CODE) &&
                   ClientId.Equals(other.ClientId) &&
                   Year == other.Year &&
                   Number == other.Number &&
                   Date == other.Date;

        }
    }
}