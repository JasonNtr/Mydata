using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataExpenseTypeDTO : MyDataEntityDTO
    {
        public int Code { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public char sign { get; set; }
    }
}
