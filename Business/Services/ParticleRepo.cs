using System;
using Domain.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ParticleRepo : Repo
    {
        public ParticleRepo(string connectionString) : base(connectionString)
        {

        }

        public async Task<List<ParticleDTO>> GetParticlesBetweenDates(DateTime startDate,DateTime endDate)
        {
            var context = GetContext();
            startDate = startDate.AddDays(-1).Date;
            endDate= endDate.AddDays(1).Date;

            var ptyppars = await context.Ptyppars.AsNoTracking().Where(x => x.UpdateMyData == 1).Select(x => x.PTYPPAR_RECR).ToListAsync();

            var particles = await context.Particles.AsNoTracking().Where(x=>x.Date > startDate.Date && x.Date < endDate
                && ptyppars.Contains(x.PTYPPAR_RECR) && x.Mark == null && x.Closed.Equals("1") && x.Printed.Equals("1")).ToListAsync();

            var particlesDTO = Mapper.Map<List<ParticleDTO>>(particles);

            var returnParticlesDTO = new List<ParticleDTO>();

            foreach (var item in particlesDTO)
            {
                returnParticlesDTO.Add(await Get(item));
            }
            
            return particlesDTO;
        }

        private async Task<ParticleDTO> Get(ParticleDTO particleDTO)
        {
            var context = GetContext();

            var branch = await context.Branches.AsNoTracking().FirstOrDefaultAsync(x => x.Code.Equals(particleDTO.Branch));
            var branchDTO = Mapper.Map<BranchDTO>(branch);
            particleDTO.BranchDTO = branchDTO;

            var ptyppar = await context.Ptyppars.AsNoTracking().FirstOrDefaultAsync(x => x.PTYPPAR_RECR.Equals(particleDTO.PTYPPAR_RECR));
            var ptypparDTO = Mapper.Map<PtypparDTO>(ptyppar);
            particleDTO.Ptyppar = ptypparDTO;

            var client = await context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId.Equals(particleDTO.ClientId));
            if (client != null)
            {
                var city = await context.Cities.AsNoTracking().FirstOrDefaultAsync(x => x.CityId.Equals(client.CLIENT_CITY_ID));
                var cityDTO = Mapper.Map<CityDTO>(city);
                var clientDTO = Mapper.Map<ClientDTO>(client);
                clientDTO.City = cityDTO;
                particleDTO.Client = clientDTO;
            }
            

            var pmoves = await context.Pmoves.AsNoTracking().Where(x => x.PARTL_RECR.Equals(particleDTO.PARTL_RECR)).ToListAsync();
            var pmovesDTO = Mapper.Map<List<PmoveDTO>>(pmoves);

            foreach (var pmove in pmovesDTO)
            {
                var item = await context.Item.AsNoTracking().FirstOrDefaultAsync(x => x.ITEM_CODE.Equals(pmove.ITEM_CODE));
                var itemDTO = Mapper.Map<ItemDTO>(item);
                var fpa = await context.FPA.AsNoTracking().FirstOrDefaultAsync(x => x.Percentage.Equals(itemDTO.FPA_POSOSTO));
                var fpaDTO = Mapper.Map<FPADTO>(fpa);
                itemDTO.FPA = fpaDTO;
                pmove.Item = itemDTO;
            }
            
            particleDTO.Pmoves = pmovesDTO;
            return particleDTO;
        }

        public async Task<bool> Update(ParticleDTO invoiceParticle)
        {
            var context = GetContext();
            var particle = await context.Particles.FirstOrDefaultAsync(x => x.Number == invoiceParticle.Number &&
                                                                            x.ClientId.Equals(invoiceParticle.ClientId)
                                                                                && x.Series.Equals(invoiceParticle
                                                                                    .Series) && x.Date ==
                                                                                invoiceParticle.Date);
            particle.Mark = invoiceParticle.Mark;
            var result = await context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<ParticleDTO> GetCancel(string itemPartlRecr)
        {
            var context = GetContext();
            var psxetiko = await context.Psxetika.FirstOrDefaultAsync(x => x.PARTL_RECR.Equals(itemPartlRecr));
            if (psxetiko?.PSX_PARTL_RECR == null) return null;

            var particle = await context.Particles.FirstOrDefaultAsync(x => x.PARTL_RECR.Equals(psxetiko.PSX_PARTL_RECR));
            return Mapper.Map<ParticleDTO>(particle);
        }

        public async Task<ParticleDTO> GetByMark(long? invoiceCancellationMark)
        {
            var context = GetContext();
            var particle = await context.Particles.FirstOrDefaultAsync(x => x.Mark.Equals(invoiceCancellationMark.ToString()));
            return Mapper.Map<ParticleDTO>(particle);
        }
    }

}
