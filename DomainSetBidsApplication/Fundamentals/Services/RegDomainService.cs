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

                            int delay = 1000 / regDomainEntity.Frequency;
                            var result = new Result<DomainAnswer> { ResultQuery = "error" };
                            do
                            {
                                token.ThrowIfCancellationRequested();

                                var dateNow = DateTime.Now;
                                if (regDomainEntity.Date >= dateNow)
                                {
                                    await Task.Delay(delay);
                                    result = await _apiFactory.Domain.SetReregBidsAsync(username, password, inputData);

                                    if (result.ResultType == ResultType.SUCCESS)
                                    {
                                        await AddLog(regDomainEntity.Name, LogType.Success, result.Answer);
                                    }
                                    else
                                    {
                                        await AddLog(regDomainEntity.Name, LogType.Error, result);
                                    }
                                }
                                else if (regDomainEntity.Date < dateNow)
                                {
                                    string msg = String.Format("Dates: {0} more {1}", regDomainEntity.Date, dateNow);

                                    await AddLog(regDomainEntity.Name, LogType.Error, msg);

                                    break;
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
