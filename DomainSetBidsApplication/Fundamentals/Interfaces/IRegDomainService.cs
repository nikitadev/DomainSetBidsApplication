using System;
using System.Threading;
using System.Threading.Tasks;
using DomainSetBidsApplication.Models;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
    public interface IRegDomainService : IService<RegDomainEntity>
    {
        Tuple<Task, CancellationTokenSource> CreateTask(RegDomainEntity regDomainentity, UserInfoEntity userInfoEntity);
    }
}