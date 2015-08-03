using System;
using System.Threading;
using System.Threading.Tasks;
using AviaTicketsWpfApplication.Models;
using DomainSetBidsApplication.Models;
using DomainSetBidsApplication.ViewModels.InteractionListeners;
using DomainSetBidsApplication.ViewModels.Pages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;

namespace DomainSetBidsApplication.ViewModels
{
    public sealed class RegDomainViewModel : ViewModelBase, IMonitorCommandBarInteractionListener
    {
        private readonly IRegDomainInteractionListener _regDomainInteractionListener;

        private bool _isTimerStop;
        private CancellationTokenSource _cancellationTokenSource;

        private TimeSpan _delayTime;
        
        private RegDomainEntity _entity;
        public RegDomainEntity Entity
        {
            get { return _entity; }
            set
            {
                Set(ref _entity, value);
                RaisePropertyChanged(() => State);
            }
        }

        private RegDomainMode _state;
        public RegDomainMode State
        {
            get { return Entity.State; }
            set
            {
                Set(ref _state, value);
                Entity.State = value;
            }
        }

        private string _delay;
        public string Delay
        {
            get { return _delay; }
            set { Set(ref _delay, value); }
        }

        public MonitorCommandBarViewModel CommandBar { get; private set; }

        public RegDomainViewModel(IRegDomainInteractionListener regDomainInteractionListener)
        {
            _regDomainInteractionListener = regDomainInteractionListener;

            CommandBar = new MonitorCommandBarViewModel(this);
        }

        private void Tick()
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() => Delay = _delayTime.ToString("c"));
            _isTimerStop = _delayTime == TimeSpan.Zero;

            _delayTime = _delayTime.Add(TimeSpan.FromSeconds(-1));
        }

        public async Task StartTimer(TimeSpan timeSpan)
        {
            _delayTime = timeSpan;

            var oneSecond = TimeSpan.FromSeconds(1);
            await Task.Run(async () => 
            {
                do
                {
                    await Task.Delay((int)oneSecond.TotalMilliseconds);

                    Tick();

                } while (!_isTimerStop);
            });
        }

        public async Task OnStartedAsync()
        {
            var tuple = await _regDomainInteractionListener.GetTaskAsync(Entity);
            if (tuple != null && tuple.Item1 != null)
            {
                _cancellationTokenSource = tuple.Item2;
                await tuple.Item1.ContinueWith(t => State = t.IsCanceled ? RegDomainMode.Cancel : t.IsFaulted ? RegDomainMode.Draft : RegDomainMode.Done).ConfigureAwait(false);
            }
        }

        public bool CanStarted()
        {
            return State != RegDomainMode.Done;
        }

        public void OnStoped()
        {
            if (_cancellationTokenSource != null)
            {
                _isTimerStop = true;

                _cancellationTokenSource.Cancel();
            }
        }

        public void OnEdited()
        {
            string param = String.Concat(AddDomainPageViewModel.ARG, "=", Entity.ToJson());

            MessengerInstance.Send(new PageMessage(typeof(AddDomainPageViewModel), param));
        }

        public async Task OnDeletedAsync()
        {
            await _regDomainInteractionListener.OnDomainRemoveAsync(Entity);
        }
    }
}
