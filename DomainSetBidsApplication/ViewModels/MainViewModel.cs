using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using AviaTicketsWpfApplication.Models;
using DomainSetBidsApplication.Fundamentals;
using DomainSetBidsApplication.Fundamentals.Interfaces;
using DomainSetBidsApplication.Models;
using DomainSetBidsApplication.ViewModels.InteractionListeners;
using DomainSetBidsApplication.ViewModels.Pages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

namespace DomainSetBidsApplication.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public sealed class MainViewModel : ViewModelBase, IRegDomainInteractionListener
    {
        // TODO: убрать создание таблиц
        private readonly Bootstrapper _bootstraper;

        private readonly ILogService _logService;
        private readonly IUserInfoService _userInfoService;
        private readonly IRegDomainService _regDomainService;

        public RelayCommand ContentLoadedCommand { get; set; }
        public RelayCommand ContentUnloadedCommand { get; set; }

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand ShowActionCommand { get; private set; }

        public RelayCommand ClearCommand { get; private set; }

        public RelayCommand<NavigatingCancelEventArgs> NavigatingCommand { get; private set; }
        public RelayCommand<NavigationEventArgs> NavigatedCommand { get; private set; }

        private ObservableCollection<RegDomainViewModel> _domains;
        public ObservableCollection<RegDomainViewModel> Domains
        {
            get { return _domains; }
            private set { Set(ref _domains, value); }
        }

        private ObservableCollection<LogEntity> _logs;
        public ObservableCollection<LogEntity> Logs
        {
            get { return _logs; }
            private set { Set(ref _logs, value); }
        }

        private bool _isDataLoaded;
        public bool IsDataLoaded
        {
            get { return _isDataLoaded; }
            set { Set(ref _isDataLoaded, value); }
        }

        private bool _isFrameVisible;
        public bool IsFrameVisible
        {
            get { return _isFrameVisible; }
            set { Set(ref _isFrameVisible, value); }
        }

        private string _pageTitle;
        public string PageTitle
        {
            get { return _pageTitle; }
            set { Set(ref _pageTitle, value); }
        }

        private bool _isSelectedOnLog;
        public bool IsSelectedOnLog
        {
            get { return _isSelectedOnLog; }
            set
            {
                if (SelectedItem != null)
                {
                    Set(ref _isSelectedOnLog, value);
                }
            }
        }

        private bool _isSettingsOpen;
        public bool IsSettingsOpen
        {
            get { return _isSettingsOpen; }
            set { Set(ref _isSettingsOpen, value); }
        }

        private bool _isCommandBarOpen;
        public bool IsCommandBarOpen
        {
            get { return _isCommandBarOpen; }
            set { Set(ref _isCommandBarOpen, value); }
        }

        private UserInfoViewModel _userInfoViewModel;
        public UserInfoViewModel UserInfo
        {
            get { return _userInfoViewModel; }
            set { Set(ref _userInfoViewModel, value); }
        }

        private RegDomainViewModel _selectedItem;
        public RegDomainViewModel SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != null)
                {
                    IsCommandBarOpen = true;
                }

                Set(ref _selectedItem, value);

                ShowActionCommand.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(
            Bootstrapper bootstraper,
            ILogService logService,
            IUserInfoService userInfoService,
            IRegDomainService regDomainService)
        {
            _bootstraper = bootstraper;

            _logService = logService;
            _userInfoService = userInfoService;
            _regDomainService = regDomainService;
            
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.

            }
            else
            {
                // Code runs "for real"

                IsDataLoaded = IsFrameVisible = false;

                ContentLoadedCommand = new RelayCommand(async () => await InitializeAsync());
                ContentUnloadedCommand = new RelayCommand(async () => await ClearAsync());

                AddCommand = new RelayCommand(AddCommandHandler);
                ClearCommand = new RelayCommand(() => Logs.Clear());
                ShowActionCommand = new RelayCommand(ShowActionCommandHandler, () => SelectedItem != null);

                NavigatingCommand = new RelayCommand<NavigatingCancelEventArgs>(NavigatingCommandHandler);
                NavigatedCommand = new RelayCommand<NavigationEventArgs>(NavigatedCommandHandler);
                
                Logs = new ObservableCollection<LogEntity>();
                Domains = new ObservableCollection<RegDomainViewModel>();

                MessengerInstance.Register<RegDomainEntity>(this, async e => await RegDomainEntityMessageHandler(e));
                MessengerInstance.Register<LogEntity>(this, async e => await LogEntityMessageHandler(e));
            }
        }

        private async Task InitializeAsync()
        {
            IsDataLoaded = true;

            await _bootstraper.InitializeAsync().ConfigureAwait(false);

            UserInfo = new UserInfoViewModel(_userInfoService);

            var items = await _userInfoService.GetAllAsync();
            if (!items.Any())
            {
                IsSettingsOpen = true;
            }

            var entities = await _regDomainService.GetAllAsync();
            foreach (var entity in entities)
            {
                await Task.Yield();
                await DispatcherHelper.RunAsync(() => Domains.Add(CreateRegDomainViewModel(entity)));
            }

            IsDataLoaded = false;
        }

        private void ShowActionCommandHandler()
        {
            IsCommandBarOpen = true;
        }

        private RegDomainViewModel CreateRegDomainViewModel(RegDomainEntity entity)
        {
            var regDomainViewModel = new RegDomainViewModel(this)
            {
                Entity = entity,
                State = RegDomainMode.Draft
            };

            return regDomainViewModel;
        }

        private async Task RegDomainEntityMessageHandler(RegDomainEntity entity)
        {
            if (entity != null)
            {
                var vm = Domains.FirstOrDefault(e => e.Entity.Id == entity.Id);
                if (vm != null)
                {
                    vm.Entity = entity;
                }
                else
                {
                    Domains.Add(CreateRegDomainViewModel(entity));
                }
            }        
        }

        private async Task LogEntityMessageHandler(LogEntity entity)
        {
            await _logService.InsertAsync(entity);
            await DispatcherHelper.RunAsync(() => Logs.Add(entity));
        }

        private void NavigatedCommandHandler(NavigationEventArgs args)
        {
            if (args.ExtraData != null)
            {
                string parametrs = args.ExtraData.ToString();
                var dicParams = parametrs.Split('&').Select(s =>
                {
                    var keyvalue = s.Split('=');

                    return new Tuple<string, string>(keyvalue[0], keyvalue[1]);
                }).ToDictionary(x => x.Item1, x => x.Item2);

                if (dicParams.Count > 0)
                {
                    PageTitle = "Edit Domain";
                    MessengerInstance.Send(new DetailsPageMessage { Parametrs = dicParams });
                }
            }
        }

        private void NavigatingCommandHandler(NavigatingCancelEventArgs args)
        {
            if (args.NavigationMode == NavigationMode.New)
            {
                IsFrameVisible = true;
            }
            else if (args.NavigationMode == NavigationMode.Back)
            {
                if (args.Uri == null)
                {
                    IsFrameVisible = false;
                }
            }

            IsCommandBarOpen = false;
        }

        private void AddCommandHandler()
        {
            PageTitle = "Add Domain";
            MessengerInstance.Send(new PageMessage(typeof(AddDomainPageViewModel)));
        }        

        private async Task ClearAsync()
        {
            await Task.Yield();

            Cleanup();
        }

        public async Task OnDomainRemoveAsync(RegDomainEntity entity)
        {
            await _regDomainService.DeleteAsync(entity.Id);
            IsCommandBarOpen = false;

            Domains.Remove(SelectedItem);
        }

        public async Task<Tuple<Task, CancellationTokenSource>> GetTaskAsync(RegDomainEntity entity)
        {
            var userInfoEntity = await _userInfoService.GetByNameAsync(entity.Register);
            if (userInfoEntity != null)
            {
                return _regDomainService.CreateTask(entity, userInfoEntity);
            }

            return null;
        }
    }
}