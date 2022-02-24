using System;

namespace Domain.Model
{
    public class MyDataEntity
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual DateTime Created { get; set; } = DateTime.UtcNow;
        public virtual DateTime Modified { get; set; } = DateTime.UtcNow;

    }
}
