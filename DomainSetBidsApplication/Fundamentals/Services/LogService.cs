using DomainSetBidsApplication.Fundamentals.Abstracts;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
using RegAPI.Library;

namespace DomainSetBidsApplication.Fundamentals.Services
{
    public sealed class LogService : BaseService<LogEntity>, ILogService
    {
        public LogService(
            IApiFactory apiFactory,
            IRepository<LogEntity> repository)
            : base(apiFactory, repository)
		{
        }
    }
}
