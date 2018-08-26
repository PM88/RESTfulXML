using System;
using RESTfulXML_1.DataAccess.Repositories;
using RESTfulXML_1.DataAccess;

namespace RESTfulXML_1.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRequestRepository Requests { get; }
        int Complete();
    }
}