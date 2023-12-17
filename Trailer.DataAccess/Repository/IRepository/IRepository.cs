using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Trailer.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        //T - Category
        //T - Movie
        IEnumerable<T> GetAll(string? includeProperties = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null);
        Task AddAsync(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
