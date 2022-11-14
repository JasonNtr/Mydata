using System;

namespace Domain.DTO
{
    public class ParticleRetries
    {
        public int Retries { get; set; }
        public decimal ParticleRec0 { get; set; }
        public DateTime LastTryDate { get; set; }
    }
}