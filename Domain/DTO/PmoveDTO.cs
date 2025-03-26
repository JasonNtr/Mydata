using System;

namespace Domain.DTO
{
    public class PMoveDTO
    {
        public decimal Code { get; set; }

         public decimal Particle { get; set; }

        public decimal? PMS_AMAFTDISC { get; set; }


        public decimal? PMS_VATAM { get; set; }

        public decimal? PMS_VATPCT { get; set; }

      
         public decimal? Quantity { get; set; }

         public decimal? UnitPrice { get; set; }

        public string? Item { get; set; }
        public string MeasurementUnitCode { get; set; }

        public decimal? POSOSTO_XARTOSH { get; set; }
        public virtual ItemDTO ItemDTO { get; set; }
        public virtual StampDutyCategoryDTO StampDutyCategory { get; set; }
        public virtual MeasurementUnitDTO MeasurementUnit { get; set; }

        public decimal? PMS_DISCAM { get; set; }
        public decimal? POSO_XARTOSH { get; set; }

        public decimal? OtherMeasurementUnitQuantity { get; set; }
        public string ItemCategory { get; set; }

        public decimal? Net1
        {
            get
            {
                var returnValue = Quantity * UnitPrice;
                return Math.Round((decimal)returnValue, 2, MidpointRounding.AwayFromZero); ;
            }
        }

        public decimal? Net2
        {
            get
            {
                return Net1 - PMS_DISCAM;
            }
        }

        public decimal? CalculatedGross
        {
            get
            {
                return Net2 + PMS_VATAM - POSO_PARAKRAT + POSO_XARTOSH;
            }
        }
        public string AADE_CODE_PARAK { get; set; }

        public decimal POSO_PARAKRAT { get; set; }
    }
}
