using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTO;
using Infrastructure.Database;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ParticleInform : IParticleInform
    {

        private readonly ApplicationDbContext ctx;
        private readonly IMapper mapper;
        public ParticleInform(ApplicationDbContext ctx, IMapper mapper)
        {
            this.ctx = ctx;
            this.mapper = mapper;
        }

        public async Task UpdateCancellationParticle(MyDataInvoiceDTO mydatainvoicedto, MyDataInvoiceDTO mydatainvoicedtobecancelled)
        {
            try
            {
                var succesfullresponse = mydatainvoicedto.MyDataCancelationResponses.Where(x => x.statusCode.Equals("Success"))
                    .OrderByDescending(x => x.Created).FirstOrDefault();
                var order = "update [PARTICLE] set AADE_MARK='@aademark' where PARTL_REC0 = @uid";
                order = order.Replace("@aademark", succesfullresponse.cancellationMark.ToString());
                order = order.Replace("@uid", mydatainvoicedto.Uid.ToString());
                await ctx.Database.ExecuteSqlRawAsync(order);

                var order2 = "update [PARTICLE] set SYSX_MARK='@aademark' where PARTL_REC0 = @uid";
                order2 = order2.Replace("@aademark", succesfullresponse.cancellationMark.ToString());
                order2 = order2.Replace("@uid", mydatainvoicedtobecancelled.Uid.ToString());
                await ctx.Database.ExecuteSqlRawAsync(order2);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateParticle(MyDataInvoiceDTO mydatainvoicedto)
        {
            try
            {
                var succesfullresponse = mydatainvoicedto.MyDataResponses.Where(x => x.statusCode.Equals("Success"))
                    .OrderByDescending(x => x.Created).FirstOrDefault();
                var order = "update [PARTICLE] set AADE_MARK='@aademark' where PARTL_REC0 = @uid";
                order = order.Replace("@aademark", succesfullresponse.invoiceMark.ToString());
                order = order.Replace("@uid", mydatainvoicedto.Uid.ToString());

                await ctx.Database.ExecuteSqlRawAsync(order);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateParticle_FixName(MyDataInvoiceDTO mydatainvoicedto)
        {
            try
            {
                var succesfullresponse = mydatainvoicedto.MyDataResponses.Where(x => x.statusCode.Equals("Success"))
                    .OrderByDescending(x => x.Created).FirstOrDefault();
                var order = "update [PARTICLE] set AADE_MARK='' where PARTL_REC0 = @uid";
                //order = order.Replace("@aademark", succesfullresponse.invoiceMark.ToString());
                order = order.Replace("@uid", mydatainvoicedto.Uid.ToString());

                await ctx.Database.ExecuteSqlRawAsync(order);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateCancellationParticle_FixName(MyDataInvoiceDTO mydatainvoicedto, MyDataInvoiceDTO mydatainvoicedtobecancelled, long? cancellationInvoiceMark)
        {
            try
            {
                //var succesfullresponse = mydatainvoicedto.MyDataCancelationResponses.Where(x => x.statusCode.Equals("Success"))
                //    .OrderByDescending(x => x.Created).FirstOrDefault();

                //var order = "update [PARTICLE] set AADE_MARK='' where PARTL_REC0 = @uid";
                ////order = order.Replace("@aademark", succesfullresponse.cancellationMark.ToString());
                //order = order.Replace("@uid", mydatainvoicedto.Uid.ToString());
                //await ctx.Database.ExecuteSqlRawAsync(order);

                var order2 = "update [PARTICLE] set SYSX_MARK='@sysxmark' where PARTL_REC0 = @uid";
                var uid = mydatainvoicedto.Uid * -1;
                //order2 = order2.Replace("@aademark", succesfullresponse.cancellationMark.ToString());
                order2 = order2.Replace("@uid", uid.ToString());
                order2 = order2.Replace("@sysxmark", cancellationInvoiceMark.ToString());
                await ctx.Database.ExecuteSqlRawAsync(order2);
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.Message);
            }
        }
    }
}
     