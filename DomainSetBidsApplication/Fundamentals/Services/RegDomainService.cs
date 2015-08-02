using System;
using System.Threading;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Abstracts;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using Newtonsoft.Json;
using RegAPI.Library;
using RegAPI.Library.Models;
using RegAPI.Library.Models.Domain;

namespace DomainSetBidsApplication.Fundamentals
{
    public sealed class RegDomainService : BaseService<RegDomainEntity>, IRegDomainService
    {
        public RegDomainService(
            IApiFactory apiFactory,
            IRepository<RegDomainEntity> repository)
            : base(apiFactory, repository)
        {
        }

        private async Task AddLog(string name, LogType type, object data)
        {
            var logEntity = new LogEntity { Name = name, Type = type, Date = DateTime.Now, Description = data.ToString() };

            await DispatcherHelper.RunAsync(() => Messenger.Default.Send(logEntity));
        }

        public Tuple<Task, CancellationTokenSource> CreateTask(RegDomainEntity regDomainEntity, UserInfoEntity userInfoEntity)
        {
            if (regDomainEntity != null)
            {
                RegisterType type;
                Enum.TryParse(regDomainEntity.Register, out type);

                Task task = null;
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                switch (type)
                {
                    case RegisterType.REG:
                        string username = userInfoEntity.Username;
                        string password = userInfoEntity.Password;
                        var contacts = JsonConvert.DeserializeObject<Contacts>(userInfoEntity.Data);

                        var inputData = new SetReregBidsInputData
                        {
                            Contacts = contacts,
                            Domains = new Domain[]
                            {
                                new Domain { Name = regDomainEntity.Name, Price = regDomainEntity.Rate }
                            },
                            NSServer = new NSServer()
                        };

                        task = Task.Run(async () =>
                        {
                            await AddLog(regDomainEntity.Name, LogType.Info, inputData);

                            var ts = regDomainEntity.Date - DateTime.Now;
                            if (ts.Ticks < 0)
                            {
                                string msg = String.Format("Dates: {0} more {1}", DateTime.Now, regDomainEntity.Date);

                                await AddLog(regDomainEntity.Name, LogType.Error, msg);

                                return;
                            }

                            var timeSpanNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                            var timeSpanStart = new TimeSpan(regDomainEntity.Hour, regDomainEntity.Minute, regDomainEntity.Second);
                            var delay = timeSpanStart - timeSpanNow;
                            if (delay.TotalMilliseconds > 0)
                            {
                                await Task.Delay((int)delay.TotalMilliseconds);
                            }

                            var result = new Result<DomainAnswer> { ResultQuery = "error" };
                            int delayFrequency = 1000 / regDomainEntity.Frequency;
                            do
                            {
                                token.ThrowIfCancellationRequested();
                                await AddLog(regDomainEntity.Name, LogType.Error, result);
                                await Task.Delay(delayFrequency);

                                result = await _apiFactory.Domain.SetReregBidsAsync(username, password, inputData);
                                if (result.ResultType == ResultType.SUCCESS)
                                {
                                    await AddLog(regDomainEntity.Name, LogType.Success, result.Answer);
                                }
                                else
                                {
                                    await AddLog(regDomainEntity.Name, LogType.Error, result);
                                }

                            } while (result.ResultType != ResultType.SUCCESS);
                        }, token);

                        break;
                    case RegisterType.NIC:
                        task = Task.Run(async () =>
                        {
                            await AddLog(regDomainEntity.Name, LogType.Error, "Not implemented yet.");
                        }, token);

                        break;
                }

                return new Tuple<Task, CancellationTokenSource>(task, tokenSource);
            }

            return null;
        }
    }
}
