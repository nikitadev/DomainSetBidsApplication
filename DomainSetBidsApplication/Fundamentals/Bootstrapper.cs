using System;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Interfaces;

namespace DomainSetBidsApplication.Fundamentals
{
	public sealed class Bootstrapper
	{
        private readonly ILogService _logService;
        private readonly IRegDomainService _regDomainService;
        private readonly IUserInfoService _userInfoService;

        public Bootstrapper(ILogService logService, IRegDomainService regDomainService, IUserInfoService userInfoService)
		{
            _logService = logService;
            _regDomainService = regDomainService;
            _userInfoService = userInfoService;
        }

		public async Task InitializeAsync(IProgress<Tuple<int, string>> progress)
		{
			progress.Report(Tuple.Create(10, "tables"));

			await _regDomainService.CreateTableAsync();
            await _logService.CreateTableAsync();
            await _userInfoService.CreateTableAsync();

            progress.Report(Tuple.Create(100, "done"));
		}

        public async Task InitializeAsync()
        {
            await _regDomainService.CreateTableAsync();
            await _logService.CreateTableAsync();
            await _userInfoService.CreateTableAsync();
        }
    }
}
