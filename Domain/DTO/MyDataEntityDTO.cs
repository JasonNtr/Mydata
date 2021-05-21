using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.DTO
{
    public class MyDataEntityDTO
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedToString
        {
            get
            {
                return Modified.ToString("dd/MM/yyyy HH:mm");
            }
        }
    }
}
