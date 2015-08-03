using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
using DomainSetBidsApplication.Properties;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;

namespace DomainSetBidsApplication.ViewModels.Pages
{
    public sealed class AddDomainPageViewModel : ViewModelBase
    {
        public const string ARG = "data";

        private readonly IRegDomainService _regDomainService;

        public RelayCommand AddOrEditCommand { get; private set; }

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

        private string _titleAddOrEditButton;
        public string TitleAddOrEditButton
        {
            get { return _titleAddOrEditButton; }
            set { Set(ref _titleAddOrEditButton, value); }
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
            }
        }

        public AddDomainPageViewModel(IRegDomainService regDomainService)
        {
            _regDomainService = regDomainService;

            MaximumFrequency = Settings.Default.MaximumFrequency;
            TickFrequency = Settings.Default.TickFrequency;

            Cleanup();

            TitleAddOrEditButton = Resources.Add;

            AddOrEditCommand = new RelayCommand(async () => await AddOrEditCommandHandler());

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

                TitleAddOrEditButton = Resources.Save;
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
                State = RegDomainMode.Draft
            };

            if (Id == 0)
            {
                await _regDomainService.InsertAsync(regDomainEntity);
            }
            else
            {
                regDomainEntity.Id = Id;
                await _regDomainService.UpdateAsync(regDomainEntity);
            }

            return regDomainEntity;
        }

        private async Task AddOrEditCommandHandler()
        {
            var entity = await CreateOrUpdateEntity();

            if (IsNow)
            {
                MessengerInstance.Send(entity, "now");
            }
            else
            {
                MessengerInstance.Send(entity);
            }

            Cleanup();
            TextMessage = IsNow 
                ? "Domain was started for register now."  
                : "Domain has been added for register.";
        }

        public override void Cleanup()
        {
            base.Cleanup();

            Id = 0;

            IsNow = true;

            var registers = Enum.GetNames(typeof(RegisterType));
            Registers = new List<string>(registers);
            Register = null;

            Name = String.Empty;
            Date = null;
            Rate = null;

            Frequency = 10;

            //StartTimeHours = DateTime.Now.Hour;
            //StartTimeMinutes = DateTime.Now.Minute;
            //StartTimeSeconds = DateTime.Now.Second;

            StartTimeHours = StartTimeMinutes = StartTimeSeconds = null;

            TextMessage = String.Empty;
        }
    }
}
