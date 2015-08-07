using System;
using System.Threading;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Abstracts;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
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

        private LogEntity CreateLog(RegDomainEntity regDomainEntity, LogType type, object data)
        {
            return new LogEntity
            {
                Name = regDomainEntity.Name,
                Register = regDomainEntity.Register,
                Rate = regDomainEntity.Rate,
                Type = type,
                Date = DateTime.Now,
                Description = data.ToString()
            };           
        }

        public Tuple<Task, CancellationTokenSource> CreateTask(
            RegDomainEntity regDomainEntity, UserInfoEntity userInfoEntity, 
            IProgress<Tuple<int, RegDomainState>> progress, IProgress<Tuple<int, TimeSpan>> progressTimer, IProgress<LogEntity> progressLogs)
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
                            progress.Report(new Tuple<int, RegDomainState>(regDomainEntity.Id, RegDomainState.Pending));
                            progressLogs.Report(CreateLog(regDomainEntity, LogType.Info, inputData));

                            if (regDomainEntity.Date.HasValue)
                            {
                                var fullDate = regDomainEntity.Date.Value;
                                fullDate = fullDate.AddHours(regDomainEntity.Hour.Value).AddMinutes(regDomainEntity.Minute.Value).AddSeconds(regDomainEntity.Second.Value);

                                var ts = fullDate - DateTime.Now;
                                if (ts.Ticks < 0)
                                {
                                    string msg = String.Format("Expired date {0:dd.MM.yyyy}", regDomainEntity.Date);
                                    progressLogs.Report(CreateLog(regDomainEntity, LogType.Error, msg));

                                    throw new TimeoutException();
                                }
                            }

                            if (regDomainEntity.Hour.HasValue && regDomainEntity.Minute.HasValue && regDomainEntity.Second.HasValue)
                            {
                                var timeSpanNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                                var timeSpanStart = new TimeSpan(regDomainEntity.Hour.Value, regDomainEntity.Minute.Value, regDomainEntity.Second.Value);
                                var delay = timeSpanStart - timeSpanNow;

                                progressTimer.Report(new Tuple<int, TimeSpan>(regDomainEntity.Id, delay));
                                if (delay.TotalMilliseconds > 0) await Task.Delay((int)delay.TotalMilliseconds, token);
                            }                            

                            var result = new Result<DomainAnswer> { ResultQuery = "error" };
                            int delayFrequency = 1000 / regDomainEntity.Frequency;
                            do
                            {
                                await Task.Yield();

                                token.ThrowIfCancellationRequested();
                                progress.Report(new Tuple<int, RegDomainState>(regDomainEntity.Id, RegDomainState.Working));
                                
                                result = await _apiFactory.Domain.SetReregBidsAsync(username, password, inputData);
                                if (result.ResultType == ResultType.SUCCESS)
                                {
                                    progressLogs.Report(CreateLog(regDomainEntity, LogType.Success, result.Answer));
                                }
                                else
                                {
                                    progressLogs.Report(CreateLog(regDomainEntity, LogType.Error, result));
                                }

                                //progress.Report(new Tuple<int, RegDomainState>(regDomainEntity.Id, RegDomainState.Pending));
                                //progressTimer.Report(new Tuple<int, TimeSpan>(regDomainEntity.Id, TimeSpan.FromMilliseconds(delayFrequency)));
                                await Task.Delay(delayFrequency, token);

                            } while (result.ResultType != ResultType.SUCCESS);
                        }, token);
                        break;                        
                    default:
                        task = Task.Run(() => progressLogs.Report(CreateLog(regDomainEntity, LogType.Error, "Not implemented yet.")), token);
                        break;
                }

                return new Tuple<Task, CancellationTokenSource>(task, tokenSource);
            }

            return null;
        }
    }
}
