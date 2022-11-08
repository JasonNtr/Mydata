using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTO;
using Domain.Model;
using Infrastructure.Interfaces.Services;

namespace Business.Services
{
    public class MyDataResponseRepo :Repo, IMyDataResponseRepo
    {
        private readonly string _connectionString;

        public MyDataResponseRepo(string connectionString) : base(connectionString)
        {

        }
      
        public async Task<int> Insert(MyDataResponseDTO mydataresponsedto)
        {
            var context = GetContext();
            var mydataresponse = new MyDataResponse();
            Mapper.Map(mydataresponsedto, mydataresponse);
            await context.MyDataResponses.AddAsync(mydataresponse);
            var result = await context.SaveChangesAsync();
            return result;
        }

        public List<MyGenericErrorsDTO> MapToGenericErrorDTO(ICollection<MyDataErrorDTO> errors)
        {
            return Mapper.Map<List<MyGenericErrorsDTO>>(errors);
        }

        public List<MyGenericErrorsDTO> MapToGenericErrorDTOCancellation(ICollection<MyDataCancelationErrorDTO> errors)
        {
            return Mapper.Map<List<MyGenericErrorsDTO>>(errors);
        }
    }
}
