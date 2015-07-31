using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DomainSetBidsApplication.ViewModels.Pages
{
    public sealed class AddDomainPageViewModel : ViewModelBase
    {
        private readonly IRegDomainService _regDomainService;

        private RegDomainEntity _regDomainEntity;

        public List<string> Registers { get; set; }

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand RunCommand { get; private set; }

        private string _domainName;
        public string DomainName
        {
            get { return _domainName; }
            set { Set(ref _domainName, value); }
        }

        private decimal _rate;
        public decimal Rate
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

        public int StartTimeHours
        {
            get { return _startTimeHours; }
            set { Set(ref _startTimeHours, value); }
        }

        public int StartTimeMinutes
        {
            get { return _startTimeMinutes; }
            set { Set(ref _startTimeMinutes, value); }
        }

        public int StartTimeSeconds
        {
            get { return _startTimeSeconds; }
            set
            {
                if (value == 60)
                {
                    value = 1;
                    StartTimeMinutes++;
                }

                Set(ref _startTimeSeconds, value);
            }
        }

        private int _deviationTimeMinute, _deviationTimeSecond;
        public int DeviationTimeMinute
        {
            get { return _deviationTimeMinute; }
            set { Set(ref _deviationTimeMinute, value); }
        }

        public int DeviationTimeSecond
        {
            get { return _deviationTimeSecond; }
            set
            {
                if (value == 60)
                {
                    value = 1;
                    DeviationTimeMinute++;
                }

                Set(ref _deviationTimeSecond, value);
            }
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

        private string _register;
        public string Register
        {
            get { return _register; }
            set { Set(ref _register, value); }
        }

        public AddDomainPageViewModel(IRegDomainService regDomainService)
        {
            _regDomainService = regDomainService;

            MaximumFrequency = 1200;

            Cleanup();

            AddCommand = new RelayCommand(async () => await AddCommandHandler());
            RunCommand = new RelayCommand(async () => await RunCommandHandler());
        }

        private void CreateEntity()
        {
            var date = Date.AddHours(StartTimeHours).AddMinutes(StartTimeMinutes).AddSeconds(StartTimeSeconds);

            _regDomainEntity = new RegDomainEntity
            {
                Name = DomainName,
                Rate = Rate,
                Register = Register,
                Date = date
            };
        }

        private async Task AddCommandHandler()
        {
            CreateEntity();

            await _regDomainService.InsertAsync(_regDomainEntity);

            MessengerInstance.Send(_regDomainEntity);

            Cleanup();

            TextMessage = "Domain had been added for register.";
        }

        private async Task RunCommandHandler()
        {
            CreateEntity();

            await _regDomainService.InsertAsync(_regDomainEntity);

            MessengerInstance.Send(_regDomainEntity);

            Cleanup();

            TextMessage = "Domain had been added and to start for register.";
        }

        public override void Cleanup()
        {
            base.Cleanup();

            var registers = Enum.GetNames(typeof(RegisterType));
            Registers = new List<string>(registers);

            Register = Registers.First();

            DomainName = String.Empty;
            Rate = 0;

            TickFrequency = 100;

            Frequency = 300;

            StartTimeHours = 9;
            StartTimeMinutes = 1;
            StartTimeSeconds = 0;

            DeviationTimeMinute = 0;
            DeviationTimeSecond = 0;

            Date = DateTime.Now;
        }
    }
}
