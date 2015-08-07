using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
using DomainSetBidsApplication.Properties;
using DomainSetBidsApplication.Utils;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;

namespace DomainSetBidsApplication.ViewModels.Pages
{
    public sealed class AddDomainPageViewModel : ViewModelBase, IDataErrorInfo
    {
        public const string ARG = "data";

        private readonly IRegDomainService _regDomainService;

        private readonly HashSet<string> _columnClearValidations;

        private bool _hasBids;

        public RelayCommand ClearCommand { get; private set; }
        public RelayCommand RunCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }

        public List<string> Registers { get; set; }

        private string _register;
        public string Register
        {
            get { return _register; }
            set { Set(ref _register, value); }
        }

        public int Id { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        private int? _rate;
        public int? Rate
        {
            get { return _rate; }
            set { Set(ref _rate, value); }
        }

        private DateTime? _date;
        public DateTime? Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        private int? _startTimeHours, _startTimeMinutes, _startTimeSeconds;

        [JsonProperty("hour")]
        public int? StartTimeHours
        {
            get { return _startTimeHours; }
            set { Set(ref _startTimeHours, value); }
        }

        [JsonProperty("minute")]
        public int? StartTimeMinutes
        {
            get { return _startTimeMinutes; }
            set { Set(ref _startTimeMinutes, value); }
        }

        [JsonProperty("second")]
        public int? StartTimeSeconds
        {
            get { return _startTimeSeconds; }
            set { Set(ref _startTimeSeconds, value); }
        }

        private int _maximumFrequency;
        public int MaximumFrequency
        {
            get { return _maximumFrequency; }
            set { Set(ref _maximumFrequency, value); }
        }

        private int _frequency, _tickFrequency;
        public int Frequency
        {
            get { return _frequency; }
            set { Set(ref _frequency, value); }
        }

        public int TickFrequency
        {
            get { return _tickFrequency; }
            set { Set(ref _tickFrequency, value); }
        }

        private string _textMessage;
        public string TextMessage
        {
            get { return _textMessage; }
            set { Set(ref _textMessage, value); }
        }

        private bool _isNow;

        [JsonIgnore]
        public bool IsNow
        {
            get { return _isNow; }
            set
            {
                if (value)
                {
                    Date = null;
                    StartTimeHours = StartTimeMinutes = StartTimeSeconds = null;
                }
                
                Set(ref _isNow, value);

                RaisePropertyChanged(nameof(Date));
                RaisePropertyChanged(nameof(StartTimeHours));
                RaisePropertyChanged(nameof(StartTimeMinutes));
                RaisePropertyChanged(nameof(StartTimeSeconds));
            }
        }

        public AddDomainPageViewModel(IRegDomainService regDomainService)
        {
            _regDomainService = regDomainService;
            _columnClearValidations = new HashSet<string>();

            _hasBids = false;

            MaximumFrequency = Settings.Default.MaximumFrequency;
            TickFrequency = Settings.Default.TickFrequency;

            Cleanup();

            ClearCommand = new RelayCommand(async () => await ClearCommandHandler());
            RunCommand = new RelayCommand(async () => await RunCommandHandler());
            SaveCommand = new RelayCommand(async () => await SaveCommandHandler(), () => String.IsNullOrEmpty(Error));
            
            MessengerInstance.Register<DetailsPageMessage>(this, DetailsPageMessageHandler);
        }

        private void DetailsPageMessageHandler(DetailsPageMessage m)
        {
            Cleanup();

            string code;
            if (m.Parametrs.TryGetValue(ARG, out code))
            {
                JsonConvert.PopulateObject(code, this);

                IsNow = !(Date.HasValue && StartTimeHours.HasValue && StartTimeMinutes.HasValue && StartTimeSeconds.HasValue);
            }
        }  

        private async Task<RegDomainEntity> CreateOrUpdateEntity()
        {
            var regDomainEntity = new RegDomainEntity
            {
                Name = Name,
                Register = Register,
                Rate = Rate.Value,
                Frequency = Frequency,
                Date = Date,
                Hour = StartTimeHours,
                Minute = StartTimeMinutes,
                Second = StartTimeSeconds,
                State = RegDomainState.Draft
            };

            if (Id == 0)
            {
                _hasBids = await _regDomainService.HasEntityByName(Name);
                if (!_hasBids)
                {
                    await _regDomainService.InsertAsync(regDomainEntity);

                    Id = regDomainEntity.Id;
                }
                else
                {
                    RaisePropertyChanged(nameof(Name));

                    return null;
                }
            }
            else
            {
                regDomainEntity.Id = Id;
                await _regDomainService.UpdateAsync(regDomainEntity);
            }

            return regDomainEntity;
        }

        private async Task ClearCommandHandler()
        {
            await SaveCommandHandler();

            Cleanup();

            TextMessage = Resources.ClearedMessage;
        }

        private async Task RunCommandHandler()
        {
            var entity = await CreateOrUpdateEntity();

            if (entity != null)
            {
                MessengerInstance.Send(entity, MessageToken.RUN);
                MessengerInstance.Send(true, MessageToken.PAGE_GO_BACK);
            }
        }

        private async Task SaveCommandHandler()
        {
            var entity = await CreateOrUpdateEntity();

            if (entity != null)
            {
                MessengerInstance.Send(entity);
                TextMessage = Resources.SavedMessage;
            }
        }

        public override void Cleanup()
        {
            base.Cleanup();

            _hasBids = false;

            Id = 0;

            IsNow = true;

            var registers = Enum.GetNames(typeof(RegisterType));
            Registers = new List<string>(registers);
            Register = null;

            Name = String.Empty;
            Date = null;
            Rate = null;

            Frequency = 10;

            StartTimeHours = StartTimeMinutes = StartTimeSeconds = null;

            TextMessage = String.Empty;
        }

        public string Error
        {
            get
            {
                return String.Join(", ", _columnClearValidations.ToArray());
            }
        }

        public string this[string columnName]
        {
            get
            {
                string result = String.Empty;
                if (columnName == nameof(Name))
                {
                    if (_hasBids)
                    {
                        result = Resources.DomainExistsValidateMessage;
                        _hasBids = false;
                    }
                    else if (String.IsNullOrEmpty(Name))
                    {
                        result = Resources.DomainValidateMessage;
                    }
                }
                if (columnName == nameof(Register))
                {
                    if (Register == null)
                        result = Resources.RegistratorValidateMessage;
                }
                if (columnName == nameof(Rate))
                {
                    if (!Rate.HasValue)
                        result = Resources.RateValidateMessage;
                }

                if (columnName == nameof(Date))
                {
                    if (!Date.HasValue && !IsNow)
                        result = Resources.DateValidateMessage;
                }
                if (columnName == nameof(StartTimeHours))
                {
                    if (!StartTimeHours.HasValue && !IsNow)
                        result = Resources.HourValidateMessage;
                }
                if (columnName == nameof(StartTimeMinutes))
                {
                    if (!StartTimeMinutes.HasValue && !IsNow)
                        result = Resources.MinuteValidateMessage;
                }
                if (columnName == nameof(StartTimeSeconds))
                {
                    if (!StartTimeSeconds.HasValue && !IsNow)
                        result = Resources.SecondValidateMessage;
                }

                if (!String.IsNullOrEmpty(result))
                {
                    _columnClearValidations.Add(columnName);
                }
                else if (_columnClearValidations.Contains(columnName))
                {
                    _columnClearValidations.Remove(columnName);
                }

                SaveCommand.RaiseCanExecuteChanged();

                return result;
            }
        }
    }
}
