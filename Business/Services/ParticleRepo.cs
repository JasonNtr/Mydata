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

            
            var ptyppars = await context.InvoiceTypes.AsNoTracking().Where(x => x.UpdateMyData == 1).Select(x => x.Code).ToListAsync();
            
            var particles = await context.Particles.AsNoTracking().Where(x=>x.Date > startDate.Date && x.Date < endDate 
                && ptyppars.Contains(x.InvoiceType) && x.Mark == null && x.Closed.Equals("1")  && x.Number>0).ToListAsync();

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
            
            var ptyppar = await context.InvoiceTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Code.Equals(particleDTO.InvoiceType));
            var ptypparDTO = Mapper.Map<InvoiceTypeDTO>(ptyppar);
            particleDTO.Ptyppar = ptypparDTO;

            var movePurpose = await context.MovePurposes.AsNoTracking().FirstOrDefaultAsync(x => x.CODE.Equals(particleDTO.SKOPDIAK));
            var movePurposeDTO = Mapper.Map<MovePurposeDTO>(movePurpose);
            particleDTO.MovePurposeDTO = movePurposeDTO;

            var client = await context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Code.Equals(particleDTO.ClientCode));
            if (client != null)
            {
                var ship = await context.Ships.AsNoTracking().FirstOrDefaultAsync(x=>x.Code == particleDTO.ShipCode);
                var shipDTO = Mapper.Map<ShipDTO>(ship);

                var clientDTO = Mapper.Map<ClientDTO>(client);
                clientDTO.Ship = shipDTO;
                particleDTO.Client = clientDTO;
            }
            
           
            var pmoves = await context.Pmoves.AsNoTracking().Where(x => x.Particle.Equals(particleDTO.Code)).ToListAsync();
            var pmovesDTO = Mapper.Map<List<PMoveDTO>>(pmoves);

            foreach (var pmove in pmovesDTO)
            {
                var item = await context.Item.AsNoTracking().FirstOrDefaultAsync(x => x.ITEM_CODE.Equals(pmove.Item));
                var itemDTO = Mapper.Map<ItemDTO>(item);
                var fpa = await context.FPA.AsNoTracking().FirstOrDefaultAsync(x => x.Percentage.Equals(itemDTO.Vat));
                var fpaDTO = Mapper.Map<FpaDTO>(fpa);
                var stampDutyCategory = await context.StampDutyCategories.AsNoTracking().FirstOrDefaultAsync(x => x.StampDuty == pmove.POSOSTO_XARTOSH);
                var stampDutyCategoryDTO = Mapper.Map<StampDutyCategoryDTO>(stampDutyCategory);
                var measurementUnit = await context.MeasurementUnits.AsNoTracking().FirstOrDefaultAsync(x => x.AME_UNIT_CODE == itemDTO.MeasurementUnitCode.ToString());
                var measurementUnitDTO = Mapper.Map<MeasurementUnitDTO>(measurementUnit);
                 
                itemDTO.FPA = fpaDTO;
                pmove.ItemDTO = itemDTO;
                pmove.StampDutyCategory = stampDutyCategoryDTO;
                pmove.MeasurementUnit = measurementUnitDTO;


            }

            particleDTO.Pmoves = pmovesDTO;
            return particleDTO;
        }

        public async Task<bool> Update(ParticleDTO invoiceParticle)
        {
            var context = GetContext();
            var particle = await context.Particles.FirstOrDefaultAsync(x => x.Code == invoiceParticle.Code  && x.Date == invoiceParticle.Date);
           
            Mapper.Map(invoiceParticle, particle);
            
            var result = await context.SaveChangesAsync();
            return result > 0;

        }

        public async Task<ParticleDTO> GetCancel(decimal cancelCode)
        {
            var context = GetContext();
            
            var particle = await context.Particles.FirstOrDefaultAsync(x => x.Code == cancelCode);
            return Mapper.Map<ParticleDTO>(particle);
        }

        public async Task<ParticleDTO> GetByMark(long? invoiceCancellationMark)
        {
            var context = GetContext();
            var particle = await context.Particles.FirstOrDefaultAsync(x => x.Mark.Equals(invoiceCancellationMark.ToString()));
            var particleDTO= Mapper.Map<ParticleDTO>(particle);

            if (particle is null) return null;
            var ptyppar = await context.InvoiceTypes.AsNoTracking().FirstOrDefaultAsync(x => x.Code.Equals(particleDTO.InvoiceType));
            var ptypparDTO = Mapper.Map<InvoiceTypeDTO>(ptyppar);
            particleDTO.Ptyppar = ptypparDTO;

            var client = await context.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Code.Equals(particleDTO.ClientCode));
            if (client != null)
            {
                
                var clientDTO = Mapper.Map<ClientDTO>(client);
                
                particleDTO.Client = clientDTO;
            }

            return particleDTO;
        }


        public async Task<ParticleDTO> GetParticleByRec0(long? invoiceUid)
        {
            var context = GetContext();
            var particle = await context.Particles.FirstOrDefaultAsync(x => x.Code == invoiceUid);
            var particleDTO = Mapper.Map<ParticleDTO>(particle);
            var fullParticle = await Get(particleDTO);
            return fullParticle;
        }

        public async Task<List<string>> GetTypes()
        {
            var context = GetContext();
            var types = await context.InvoiceTypes.Select(x => x.TYPOS_XARAKTHR).Where(x => x != null).Distinct().OrderBy(x => x).ToListAsync();
            return types;
        }

        public async Task<ParticleDTO> GetCancel(decimal? cancelledBy)
        {
            var context = GetContext();
            var particle = await context.Particles.FirstOrDefaultAsync(x => x.Code == cancelledBy);
            var particleDTO = Mapper.Map<ParticleDTO>(particle);
             
            return particleDTO;
        }
    }
    

}
