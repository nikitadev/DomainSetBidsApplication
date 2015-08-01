using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
    public interface IService<T> where T : IDbEntity, new()
    {
        Task CreateTableAsync();

        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        Task<List<T>> GetAllAsync();

        Task<T> GetAsync(int id);
    }
}
