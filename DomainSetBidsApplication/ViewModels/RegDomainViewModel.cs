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
        private CancellationTokenSource _cancellationTokenSource, _cancellationTokenSourceTimer;
        
        private RegDomainEntity _entity;
        public RegDomainEntity Entity
        {
            get { return _entity; }
            set
            {
                Set(ref _entity, value);
                RaisePropertyChanged(nameof(State));
            }
        }

        private RegDomainState _state;
        public RegDomainState State
        {
            get { return Entity.State; }
            set
            {
                Set(ref _state, value);
                Entity.State = value;

                RaisePropertyChanged(nameof(StateLocalName));
            }
        }

        public string StateLocalName
        {
            get { return State.ToLocalString(); }
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

        public async Task StartTimer(TimeSpan timeSpan)
        {
            var delayTime = timeSpan;

            var oneSecond = TimeSpan.FromSeconds(1);
            var oneBackSecond = TimeSpan.FromSeconds(-1);

            _cancellationTokenSourceTimer = new CancellationTokenSource();
            var token = _cancellationTokenSourceTimer.Token;
            await Task.Run(async () => 
            {
                do
                {
                    await Task.Delay((int)oneSecond.TotalMilliseconds);

                    delayTime = delayTime.Add(oneBackSecond);
                    await DispatcherHelper.RunAsync(() => Delay = delayTime.ToString("c"));

                    _isTimerStop = delayTime == TimeSpan.Zero;

                    if (token.IsCancellationRequested)
                    {
                        token.ThrowIfCancellationRequested();
                    }

                } while (!_isTimerStop);
            }, token)
            .ContinueWith(t => _isTimerStop = t.IsCanceled);
        }

        public async Task OnStartedAsync()
        {
            var tuple = await _regDomainInteractionListener.GetTaskAsync(Entity);
            if (tuple != null && tuple.Item1 != null)
            {
                _cancellationTokenSource = tuple.Item2;
                await tuple.Item1.ContinueWith(t => 
                    State = t.IsCanceled ? RegDomainState.Cancel 
                        : t.IsFaulted ? RegDomainState.None 
                        : RegDomainState.Done
                ).ConfigureAwait(false);

                await _regDomainInteractionListener.OnSaveStateAsync(Entity);
            }
        }

        public bool CanStarted()
        {
            return State != RegDomainState.Done;
        }

        public void OnStoped()
        {
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }

            if (_cancellationTokenSourceTimer != null)
            {
                _cancellationTokenSourceTimer.Cancel();
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

        public override string ToString()
        {
            return Entity.ToString();
        }
    }
}
