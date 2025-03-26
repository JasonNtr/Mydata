using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class MeasurementUnitDTO
    {
        public decimal CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string AME_UNIT_CODE { get; set; }

        public string AME_UNIT_DESCR { get; set; }

    }
}
