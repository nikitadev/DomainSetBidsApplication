using System.Collections.Generic;
using System.Threading.Tasks;
using DomainSetBidsApplication.Models;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
    public interface ILogService : IService<LogEntity>
    {
        Task<List<LogEntity>> GetLogsByNameAsync(string name);
    }
}
