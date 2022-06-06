using AnL.Repository.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AnL.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _Context;
        //private readonly OrderContext _Context;
        DbSet<T> dbSet;
        //DbQuery<T> queries;

        //private RawSqlString spQuery;
        //return queries.FromSql(spQuery).ToListAsync();

        public Repository(DbContext context)
        {
            _Context = context;
            dbSet = context.Set<T>();
            //queries = _Context.Query<T>();
        }

        public virtual T GetById(object Id)
        {
            return dbSet.Find(Id);
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public virtual IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> expression)
        {
            return this.dbSet.Where(expression).AsNoTracking();
        }

        public void Add(T model)
        {
            dbSet.Add(model);
        }

        public void Detached(T model)
        {
            _Context.Entry<T>(model).State = EntityState.Detached;
        }

        public void Modify(T model)
        {
            _Context.Entry<T>(model).State = EntityState.Modified;
        }



        public void Modify(IEnumerable<T> modelList)
        {
            foreach (var model in modelList)
                _Context.Entry<T>(model).State = EntityState.Modified;

        }

        public void Delete(T model)
        {
            dbSet.Remove(model);
        }

        public void DeleteById(object Id)
        {
            var model = this.GetById(Id);
            this.Delete(model);
        }

        public void SaveChanges()
        {
            _Context.SaveChanges();
        }
    }
}
