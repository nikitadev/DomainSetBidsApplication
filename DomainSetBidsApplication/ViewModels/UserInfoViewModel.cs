using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using RegAPI.Library.Models;
using RegAPI.Library.Models.Domain;

namespace DomainSetBidsApplication.ViewModels
{
    public sealed class UserInfoViewModel : ViewModelBase
    {
        private readonly IUserInfoService _userInfoService;

        public List<string> Registers { get; private set; }

        public RelayCommand CheckCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand ClearCommand { get; private set; }

        public RelayCommand PopulateTestDataCommand { get; private set; }
        public RelayCommand PopulateTestLoginCommand { get; private set; }

        public RelayCommand<string> PopulateCommand { get; set; }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        private string _checkResult;
        public string CheckResult
        {
            get { return _checkResult; }
            set { Set(ref _checkResult, value); }
        }

        private string _description;

        [JsonProperty("descr")]
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        private string _person;
        public string Person
        {
            get { return _person; }
            set { Set(ref _person, value); }
        }

        private string _personLocalName;

        [JsonProperty("person_r")]
        public string PersonLocalName
        {
            get { return _personLocalName; }
            set { Set(ref _personLocalName, value); }
        }

        private string _passport;
        public string Passport
        {
            get { return _passport; }
            set { Set(ref _passport, value); }
        }

        private DateTime? _birthDate;

        [JsonProperty("birth_date")]
        public DateTime? BirthDate
        {
            get { return _birthDate; }
            set { Set(ref _birthDate, value); }
        }

        private string _personAddress;

        [JsonProperty("p_addr")]
        public string PersonAddress
        {
            get { return _personAddress; }
            set { Set(ref _personAddress, value); }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set { Set(ref _phone, value); }
        }

        private string _email;

        [JsonProperty("e_mail")]
        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        private string _country;
        public string Country
        {
            get { return _country; }
            set { Set(ref _country, value); }
        }

        private string _register;
        public string Register
        {
            get { return _register; }
            set
            {
                PopulateCommand.Execute(value);

                Set(ref _register, value);
            }
        }

        public UserInfoViewModel(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;

            PopulateCommand = new RelayCommand<string>(async (r) => await PopulateAsync(r));

            var registers = Enum.GetNames(typeof(RegisterType));
            Registers = new List<string>(registers);

            Register = Registers.First();
            
            CheckCommand = new RelayCommand(async () => await CheckCommandHandler());
            ClearCommand = new RelayCommand(ClearCommandHandler);

            PopulateTestLoginCommand = new RelayCommand(PopulateTestLoginCommandHandler);
            PopulateTestDataCommand = new RelayCommand(PopulateTestDataCommandHandler);

            SaveCommand = new RelayCommand(async () => await SaveCommandHandler());       
        }

        private void ClearCommandHandler()
        {
            Phone = Email = PersonAddress = PersonLocalName = Description = Person = Passport = Country = String.Empty;

            BirthDate = null;
        }

        private void PopulateTestLoginCommandHandler()
        {
            Username = DefaultUserSettings.UserName;
            Password = DefaultUserSettings.Password;
        }

        private void PopulateTestDataCommandHandler()
        {
            var contacts = DefaultUserSettings.GetContacts();

            Description = contacts.Description;
            Person = contacts.Person;
            PersonLocalName = contacts.PersonLocalName;
            BirthDate = contacts.BirthDate;
            Passport = contacts.Passport;
            PersonAddress = contacts.PersonAddress;
            Phone = contacts.Phone;
            Email = contacts.Email;
            Country = contacts.Country;
        }

        public async Task PopulateAsync(string register)
        {
            var entity = await _userInfoService.GetByNameAsync(register);
            if (entity != null)
            {
                Username = entity.Username;
                Password = entity.Password;

                JsonConvert.PopulateObject(entity.Data, this);
            }
            else
            {
                Username = Password = String.Empty;

                ClearCommandHandler();
            }
        }

        private async Task SaveCommandHandler()
        {
            var inputData = new Contacts
            {
                Description = Description,
                Person = Person,
                PersonLocalName = PersonLocalName,
                Passport = Passport,
                Email = Email,
                Phone = Phone,
                BirthDate = BirthDate.Value,
                Country = Country,
                PersonAddress = PersonAddress
            };

            var userInfoEntity = new UserInfoEntity
            {
                Name = Register,
                Username = Username,
                Password = Password,
                Data = inputData.ToString()
            };

            var entity = await _userInfoService.GetByNameAsync(Register);
            if (entity != null)
            {
                userInfoEntity.Id = entity.Id;
                await _userInfoService.UpdateAsync(userInfoEntity);
            }
            else
            {
                await _userInfoService.InsertAsync(userInfoEntity);
            }
        }

        private async Task CheckCommandHandler()
        {
            if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
            {
                var result = await _userInfoService.CheckAutorization(Username, Password);

                if (result.ResultType == ResultType.ERROR)
                {
                    CheckResult = (result as Error).ToString();
                }
                else
                {
                    CheckResult = result.Answer.ToString();
                }
            }
        }
    }
}
