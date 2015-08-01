using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
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
        public RelayCommand RunCommand { get; private set; }

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

        private int _rate;
        public int Rate
        {
            get { return _rate; }
            set { Set(ref _rate, value); }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { Set(ref _date, value); }
        }

        private int _startTimeHours, _startTimeMinutes, _startTimeSeconds;

        [JsonProperty("hour")]
        public int StartTimeHours
        {
            get { return _startTimeHours; }
            set { Set(ref _startTimeHours, value); }
        }

        [JsonProperty("minute")]
        public int StartTimeMinutes
        {
            get { return _startTimeMinutes; }
            set { Set(ref _startTimeMinutes, value); }
        }

        [JsonProperty("second")]
        public int StartTimeSeconds
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

        public AddDomainPageViewModel(IRegDomainService regDomainService)
        {
            _regDomainService = regDomainService;

            MaximumFrequency = 1200;

            Cleanup();

            TitleAddOrEditButton = "Add";

            AddOrEditCommand = new RelayCommand(async () => await AddOrEditCommandHandler());
            RunCommand = new RelayCommand(async () => await RunCommandHandler());

            MessengerInstance.Register<DetailsPageMessage>(this, DetailsPageMessageHandler);
        }

        private void DetailsPageMessageHandler(DetailsPageMessage m)
        {
            Cleanup();

            string code;
            if (m.Parametrs.TryGetValue(ARG, out code))
            {
                JsonConvert.PopulateObject(code, this);

                TitleAddOrEditButton = "Save";
            }
        }

        private async Task<RegDomainEntity> CreateOrUpdateEntity()
        {
            var date = Date.AddHours(StartTimeHours).AddMinutes(StartTimeMinutes).AddSeconds(StartTimeSeconds);

            var regDomainEntity = new RegDomainEntity
            {
                Name = Name,
                Register = Register,
                Rate = Rate,
                Frequency = Frequency,
                Date = date,
                Hour = StartTimeHours,
                Minute = StartTimeMinutes,
                Second = StartTimeSeconds
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
            
            Cleanup();
            MessengerInstance.Send(entity);

            TextMessage = "Domain has been added for register.";
        }

        private async Task RunCommandHandler()
        {
            await AddOrEditCommandHandler();

            TextMessage = "Domain has been added and to start for register.";
        }

        public override void Cleanup()
        {
            base.Cleanup();

            var registers = Enum.GetNames(typeof(RegisterType));
            Registers = new List<string>(registers);

            Register = Registers.First();

            Name = String.Empty;
            Rate = 0;

            TickFrequency = 100;

            Frequency = 300;

            Date = DateTime.Now;

            StartTimeHours = DateTime.Now.Hour;
            StartTimeMinutes = DateTime.Now.Minute;
            StartTimeSeconds = DateTime.Now.Second;

            TextMessage = String.Empty;
        }
    }
}
