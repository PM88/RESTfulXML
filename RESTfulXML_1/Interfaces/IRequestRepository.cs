using RESTfulXML_1.DataAccess.Repositories;
using RESTfulXML_1.Models;

namespace RESTfulXML_1.Interfaces
{
    public interface IRequestRepository : IRepository<Request>
    {
        Request GetRequest(params object[] keys);
    }
}