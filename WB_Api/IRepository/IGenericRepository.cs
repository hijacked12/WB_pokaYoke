using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WB_Api.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null
            );

        Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null);
        void Update(T entity);
        Task Delete(int id);
        void DeleteRange(IEnumerable<T> entities);
        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entities);
    }
}
