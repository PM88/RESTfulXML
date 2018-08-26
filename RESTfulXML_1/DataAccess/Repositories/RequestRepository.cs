using RESTfulXML_1.Models;
using RESTfulXML_1.Interfaces;
using System;
using System.Data.Entity.Validation;

namespace RESTfulXML_1.DataAccess.Repositories
{
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        public RequestRepository(ApplicationContext context) : base(context)
        {
        }

        public Request GetRequest(params object[] keys)
        {
            return DbSet.Find(keys);
        }
    }
}