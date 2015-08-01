using System;
using System.Threading;
using System.Threading.Tasks;
using AviaTicketsWpfApplication.Models;
using DomainSetBidsApplication.Models;
using DomainSetBidsApplication.ViewModels.InteractionListeners;
using DomainSetBidsApplication.ViewModels.Pages;
using GalaSoft.MvvmLight;

namespace DomainSetBidsApplication.ViewModels
{
    public sealed class RegDomainViewModel : ViewModelBase, IMonitorCommandBarInteractionListener
    {
        private readonly IRegDomainInteractionListener _regDomainInteractionListener;

        private CancellationTokenSource _cancellationTokenSource;

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

        public MonitorCommandBarViewModel CommandBar { get; private set; }

        public RegDomainViewModel(IRegDomainInteractionListener regDomainInteractionListener)
        {
            _regDomainInteractionListener = regDomainInteractionListener;

            CommandBar = new MonitorCommandBarViewModel(this);
        }

        public async Task OnStartedAsync()
        {
            var tuple = await _regDomainInteractionListener.GetTaskAsync(Entity);
            if (tuple != null && tuple.Item1 != null)
            {
                _cancellationTokenSource = tuple.Item2;

                State = RegDomainMode.Work;
                await tuple.Item1.ContinueWith(t =>
                {
                    if (t.IsCanceled)
                    {
                        State = RegDomainMode.Cancel;
                    }
                    else if (t.IsCompleted)
                    {
                        //var logEntity = new LogEntity { Name = SelectedItem.Entity.Name };
                        //logEntity.Type = LogType.Success;

                        //await DispatcherHelper.RunAsync(() => Logs.Add(logEntity));

                        State = RegDomainMode.Done;
                    }

                }).ConfigureAwait(false);
            }
        }

        public void OnStoped()
        {
            if (_cancellationTokenSource != null)
            {
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
