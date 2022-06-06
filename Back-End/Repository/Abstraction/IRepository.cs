using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AnL.Repository.Abstraction
{
    public interface IRepository<T> where T : class
    {
        void Add(T model);
        IQueryable<T> GetAll();
        IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> expression);
        T GetById(object Id);
        void Modify(T model);

        void Detached(T model);
        void Modify(IEnumerable<T> modelList);
        void Delete(T model);
        void DeleteById(object Id);
    }
}
