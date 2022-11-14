using Business.Services;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Business.Brokers
{
    public class ParticleBroker
    {
        private readonly string _connectionString;
        private readonly DateTime _dateFrom;
        private readonly DateTime _dateTo;

        public List<ParticleDTO> Particles { get; set; } = new List<ParticleDTO>();
       
        private readonly Timer _timer;

        public event EventHandler<bool> ProductionChangeHappened;

        public ParticleBroker(string connectionString, DateTime dateFrom, DateTime dateTo)
        {
            _connectionString = connectionString;
            _dateFrom = dateFrom;
            _dateTo = dateTo;

            _timer = new Timer();
            _timer.Elapsed += CheckForChanges;
            _timer.Interval = 5000;
        }
        
        public ParticleBroker(string connectionString, DateTime dateFrom, DateTime dateTo, List<ParticleDTO> particleDtos)
        {
            _connectionString = connectionString;
            _dateFrom = dateFrom;
            _dateTo = dateTo;

            _timer = new Timer();
            _timer.Elapsed += CheckForChanges;
            _timer.Interval = 5000;

            Particles = particleDtos;
        }

        private async void CheckForChanges(object sender, ElapsedEventArgs e)
        {
            var particleRepo = new ParticleRepo(_connectionString);
            var particles = await particleRepo.GetParticlesBetweenDates(_dateFrom,_dateTo);
            var changeHappened = Particles.SequenceEqual(particles);
            if (changeHappened || (particles.Count == 0 && Particles.Count == 0))
                return;
            Debug.WriteLine("Production Request Database Change Happened");
            Particles = particles;
            ProductionChangeHappened?.Invoke(this, true);
        }

        public void Start() => _timer.Start();

        public void Close()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        ~ParticleBroker()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        public async Task Refresh()
        {
            var dailyProductionRepo = new ParticleRepo(_connectionString);
            var productions = await dailyProductionRepo.GetParticlesBetweenDates(_dateFrom, _dateTo);
            Particles = productions;
        }

        public void Stop() => _timer.Stop();
    }
}