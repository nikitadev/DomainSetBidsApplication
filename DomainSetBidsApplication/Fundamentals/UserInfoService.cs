using System;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Abstracts;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
using RegAPI.Library;

namespace DomainSetBidsApplication.Fundamentals
{
    public sealed class UserInfoService : BaseService<UserInfoEntity>, IUserInfoService
    {
        public UserInfoService(
            IApiFactory apiFactory,
            IRepository<UserInfoEntity> repository)
            : base(apiFactory, repository)
		{
        }

        public Task<UserInfoEntity> GetByNameAsync(string name)
        {
            var items = _repository.Table.Where(t => t.Name.Equals(name));

            return items.FirstOrDefaultAsync();
        }
    }
}
