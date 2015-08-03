using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
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

        private DispatcherTimer _timer;
        private CancellationTokenSource _cancellationTokenSource;

        private TimeSpan _delayTime;
        
        private RegDomainEntity _entity;
        public RegDomainEntity Entity
        {
            get { return _entity; }
            set { Set(ref _entity, value); }
        }

        private RegDomainMode _state;
        public RegDomainMode State
        {
            get { return _state; }
            set { Set(ref _state, value); }
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

        private void Tick(object sender, EventArgs e)
        {
            Delay = _delayTime.ToString("c");
            if (_delayTime == TimeSpan.Zero)
                _timer.Stop();

            _delayTime = _delayTime.Add(TimeSpan.FromSeconds(-1));
        }

        public void StartTimer(TimeSpan timeSpan)
        {
            _delayTime = timeSpan;
            _timer = new DispatcherTimer(TimeSpan.FromSeconds(1), DispatcherPriority.Normal, Tick, DispatcherHelper.UIDispatcher);
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

        public void OnStoped()
        {
            if (_cancellationTokenSource != null)
            {
                if (_timer != null) _timer.Stop();

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
