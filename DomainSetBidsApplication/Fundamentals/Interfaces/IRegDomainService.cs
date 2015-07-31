using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DomainSetBidsApplication.Models;
using RegAPI.Library.Models;
using RegAPI.Library.Models.Autorization;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
	public interface IRegDomainService
    {
		Task CreateTableAsync();

        Task InsertAsync(RegDomainEntity entity);
        Task UpdateAsync(RegDomainEntity entity);
		Task DeleteAsync(int id);

        Task<List<RegDomainEntity>> GetAllAsync();

        Task<RegDomainEntity> GetAsync(int id);

        Tuple<Task, CancellationTokenSource> CreateTask(RegDomainEntity regDomainentity, UserInfoEntity userInfoEntity);

        Task<Result<AutorizationAnswer>> CheckAutorization(string username, string password);
    }
}