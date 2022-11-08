using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class EntityDTO
    {

        public virtual Guid Id { get; set; } = Guid.NewGuid();

        public virtual DateTime Created { get; set; } = DateTime.Now;

        public virtual DateTime Modified { get; set; } = DateTime.Now;

        public virtual Guid? UserId { get; set; }

        public virtual bool Active { get; set; } = true;

        public virtual bool Deleted { get; set; } = false;
    }
}
