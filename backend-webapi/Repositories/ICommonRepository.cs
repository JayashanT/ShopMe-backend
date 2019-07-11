using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace webapi.Repositories
{
    public interface ICommonRepository<TEntity>
    {
        void Add(TEntity entity);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll();
        void Remove(TEntity entity);
        bool Save();
        void Update(TEntity entity);
    }
}