using System.Collections.Generic;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using RegAPI.Library;

namespace DomainSetBidsApplication.Fundamentals.Abstracts
{
    public abstract class BaseService<T> where T : IDbEntity, new()
    {
        protected readonly IApiFactory _apiFactory;
        protected readonly IRepository<T> _repository;

        public BaseService(
            IApiFactory apiFactory, 
            IRepository<T> repository)
        {
            _apiFactory = apiFactory;
            _repository = repository;
        }

        public async Task CreateTableAsync()
        {
            if (!await _repository.Connection.ExistsAsync<T>())
            {
                await _repository.Connection.CreateTableAsync<T>();
            }
        }

        public virtual async Task InsertAsync(T entity)
        {
            await _repository.InsertAsync(entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var item = GetAsync(entity.Id);
            if (item != null)
            {
                await _repository.UpdateAsync(entity);
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            var item = await GetAsync(id);
            if (item != null)
            {
                await _repository.DeleteAsync(item);
            }
        }

        public Task<List<T>> GetAllAsync()
        {
            return _repository.Table.ToListAsync();
        }

        public Task<T> GetAsync(int id)
        {
            var items = _repository.Table.Where(t => t.Id == id);

            return items.FirstOrDefaultAsync();
        }
    }
}
