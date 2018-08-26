using System;
using System.Collections.Generic;
using System.Linq.Expressions;


namespace RESTfulXML_1.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        void Add(IEnumerable<T> entries);
    }
}