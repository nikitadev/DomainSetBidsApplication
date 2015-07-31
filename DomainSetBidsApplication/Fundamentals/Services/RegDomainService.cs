using System;
using System.Collections.Generic;
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
using RegAPI.Library.Models.Autorization;
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

        public async Task<Result<AutorizationAnswer>> CheckAutorization(string username, string password)
        {
            var result = await _apiFactory.Autorization.CheckAsync(username, password);

            return result;
        }

        public Tuple<Task, CancellationTokenSource> CreateTask(RegDomainEntity regDomainentity, UserInfoEntity userInfoEntity)
        {
            if (regDomainentity != null)
            {
                RegisterType type;
                Enum.TryParse(regDomainentity.Register, out type);

                switch (type)
                {
                    case RegisterType.REG:
                        string username = userInfoEntity.Username;
                        string password = userInfoEntity.Password;
                        var contacts = JsonConvert.DeserializeObject<Contacts>(userInfoEntity.Data);
                            /*new Contacts
                        {
                            Description = "Vschizh site",
                            Person = "Svyatoslav V Ryurik",
                            PersonLocalName = "Рюрик Святослав Владимирович",
                            PassportContent = "22 44 668800, выдан по месту жилья 01.09.1984",
                            BirthDate = new DateTime(1984, 9, 1),
                            PersonAddress = "12345, г. Вщиж, ул. Княжеска, д.1, Рюрику Святославу Владимировичу, князю Вщижскому",
                            Phone = "+7 495 1234567",
                            Email = "test@test.ru",
                            Country = "RU"
                        };*/

                        var inputData = new SetReregBidsInputData
                        {
                            Contacts = contacts,
                            Domains = new Domain[]
                            {
                                new Domain { Name = regDomainentity.Name, Price = regDomainentity.Rate }
                            },
                            NSServer = new NSServer()
                        };

                        var tokenSource = new CancellationTokenSource();
                        var token = tokenSource.Token;
                        var task = Task.Run(async () =>
                        {
                            var result = new Result<DomainAnswer> { ResultQuery = "error" };
                            do
                            {
                                token.ThrowIfCancellationRequested();

                                var dateNow = DateTime.Now;
                                var logEntity = new LogEntity { Name = regDomainentity.Name };
                                if (regDomainentity.Date >= dateNow)
                                {
                                    await Task.Delay(regDomainentity.Frequency);
                                    result = await _apiFactory.Domain.SetReregBidsAsync(username, password, inputData);
                                    if (result.ResultType == ResultType.SUCCESS)
                                    {
                                        logEntity.Type = LogType.Success;
                                        logEntity.Description = result.Answer.ToString();
                                    }
                                    else
                                    {
                                        logEntity.Type = LogType.Error;
                                        logEntity.Description = (result as Error).ToString();
                                    }

                                    await DispatcherHelper.RunAsync(() => Messenger.Default.Send(logEntity));
                                }
                                else if (regDomainentity.Date < dateNow)
                                {
                                    logEntity.Type = LogType.Error;

                                    logEntity.Description = String.Format("Date: {0} < Now {1}", regDomainentity.Date, dateNow);

                                    await DispatcherHelper.RunAsync(() => Messenger.Default.Send(logEntity));

                                    break;
                                }
                            } while (result.ResultType != ResultType.SUCCESS);
                        }, token);
                        
                        return new Tuple<Task, CancellationTokenSource>(task, tokenSource);
                }
            }

            return null;
        }
    }
}
