using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using RESTfulXML_1.Interfaces;
using System.Data.Entity.Validation;

namespace RESTfulXML_1.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext Context;

        protected readonly DbSet<T> DbSet;

        protected string errorMessage = string.Empty;

        public Repository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public void Add(IEnumerable<T> entries)
        {
            DbSet.AddRange(entries);
            Context.SaveChanges();
        }
    }
}