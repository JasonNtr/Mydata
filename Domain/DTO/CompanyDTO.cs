using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class CompanyDTO  
    {

        public string? CompanyId { get; set; } = "01";
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Zipcode { get; set; }
        public string? VatNumber { get; set; }

        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Fax { get; set; }
        public string? Responsible { get; set; }


        public string? PostOffice { get; set; }
        public string? Email { get; set; }
        public double? Temperature { get; set; } = 25;

        public int? CityCode { get; set; }
        
    }
}
