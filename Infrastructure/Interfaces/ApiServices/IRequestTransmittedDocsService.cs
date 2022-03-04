using Infrastructure.Database.RequestDocModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infrastructure.Interfaces.ApiServices
{
    public interface IRequestTransmittedDocsService
    {
        public Task<int> RequestDocsFirstImplementation(string mark);
        public Task<int> RequestDocs(string mark);
        public Task<int> RequestTransmittedDocs(string mark);
        public Task<string> CallRequestDocsMethod(string mark, ContinuationToken continuationToken);
        public Task<string> CallRequestTransmittedDocsMethod(string mark, ContinuationToken continuationToken);
        public Task<ContinuationToken> ConvertRequestedDocsToDTO(RequestedDoc requestedDocs);
        public RequestedDoc DeserializeXml(XmlDocument doc);
    }
}
