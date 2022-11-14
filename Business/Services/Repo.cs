using AutoMapper;
using Infrastructure.Database;
using System;
using Business.Mappings;

namespace Business.Services
{
    public class Repo : IDisposable
    {
        private bool _disposed;
        protected readonly IMapper Mapper;
        private readonly ApplicationDbContext _context;
 
        private readonly string _connectionString;
        private readonly bool _isTransaction;

        protected Repo(string connectionString)
        {
            _connectionString = connectionString;
            var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfiles()); });
            Mapper = new AutoMapper.Mapper(mapperConfig);
            _context = new ApplicationDbContext(connectionString);
            _isTransaction = false;
        }

     

        protected ApplicationDbContext GetContext()
        {
            return _isTransaction ? _context : new ApplicationDbContext(_connectionString);
        }

         
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
}
