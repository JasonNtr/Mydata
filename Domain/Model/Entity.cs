using System;

namespace Domain.Model
{   
    public class Entity
    {

        public virtual Guid Id { get; set; } = Guid.NewGuid();

        public virtual DateTime Created { get; set; } = DateTime.Now;

        public virtual DateTime Modified { get; set; } = DateTime.Now;

        public virtual Guid? UserId { get; set; }

        public virtual bool Active { get; set; } = true;

        public virtual bool Deleted { get; set; } = false;
    }
}
