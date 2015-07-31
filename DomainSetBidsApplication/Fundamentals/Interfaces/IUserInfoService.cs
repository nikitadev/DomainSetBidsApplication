using System.Collections.Generic;
using System.Threading.Tasks;
using DomainSetBidsApplication.Models;
using DomainSetBidsApplication.ViewModels;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
    public interface IUserInfoService
    {
        Task CreateTableAsync();

        Task InsertAsync(UserInfoEntity entity);

        Task<List<UserInfoEntity>> GetAllAsync();

        Task<UserInfoEntity> GetAsync(int id);

        Task<UserInfoEntity> GetByNameAsync(string name);
    }
}
