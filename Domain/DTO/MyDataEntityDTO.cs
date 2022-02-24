using System;

namespace Domain.DTO
{
    public class MyDataEntityDTO
    {
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        public virtual DateTime Created { get; set; } = DateTime.UtcNow;
        public virtual DateTime Modified { get; set; } = DateTime.UtcNow;
        public virtual string ModifiedToString
        {
            get
            {
                return Modified.ToString("dd/MM/yyyy HH:mm");
            }
        }
    }
}
