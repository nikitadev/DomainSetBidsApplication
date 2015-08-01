using System.Threading.Tasks;
using DomainSetBidsApplication.Models;
using RegAPI.Library.Models;
using RegAPI.Library.Models.Autorization;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
    public interface IUserInfoService : IService<UserInfoEntity>
    {
        Task<UserInfoEntity> GetByNameAsync(string name);

        Task<Result<AutorizationAnswer>> CheckAutorization(string username, string password);
    }
}
