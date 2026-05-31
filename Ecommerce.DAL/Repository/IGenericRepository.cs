using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string[]? includes = null);

        Task<T> CreateAsync(T category);
        Task<bool> UpdateAsync(T entity);
        Task<T?> GetOne(Expression<Func<T, bool>> filter, string[]? includes = null);
        Task<bool> DeleteAysnc(T entity);
        Task<bool> DeleteRangeAsync(List<T> entities);

    }
}
