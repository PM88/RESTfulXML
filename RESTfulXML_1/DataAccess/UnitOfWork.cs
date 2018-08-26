using RESTfulXML_1.DataAccess.Repositories;
using RESTfulXML_1.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RESTfulXML_1.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            Requests = new RequestRepository(_context);
        }

        public IRequestRepository Requests { get; private set; }
        private readonly ApplicationContext _context;

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}