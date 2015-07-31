using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainSetBidsApplication.Models;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
    public interface ILogService
    {
        Task CreateTableAsync();

        Task InsertAsync(LogEntity entity);

        Task<List<LogEntity>> GetAllAsync();

        Task<LogEntity> GetAsync(int id);
    }
}
