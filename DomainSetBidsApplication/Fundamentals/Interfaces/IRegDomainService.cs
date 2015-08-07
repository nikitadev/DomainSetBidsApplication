using System;
using System.Threading;
using System.Threading.Tasks;
using DomainSetBidsApplication.Models;

namespace DomainSetBidsApplication.Fundamentals.Interfaces
{
    public interface IRegDomainService : IService<RegDomainEntity>
    {
        Task<bool> HasEntityByName(string name);

        Tuple<Task, CancellationTokenSource> CreateTask(
            RegDomainEntity regDomainentity, 
            UserInfoEntity userInfoEntity,
            IProgress<Tuple<int, RegDomainState>> progress,
            IProgress<Tuple<int, TimeSpan>> progressTimer,
            IProgress<LogEntity> progressLogs);
    }
}