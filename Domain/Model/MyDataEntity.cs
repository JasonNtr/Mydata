using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
    public class MyDataEntity
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Modified { get; set; }

    }
}
