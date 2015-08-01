using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainSetBidsApplication.ViewModels.InteractionListeners;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace DomainSetBidsApplication.ViewModels
{
    public sealed class MonitorCommandBarViewModel : ViewModelBase
    {
        private readonly IMonitorCommandBarInteractionListener _monitorCommandBarInteractionListener;

        private bool _isStartCommandPressed;

        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }

        public RelayCommand StartCommand { get; private set; }
        public RelayCommand StopCommand { get; private set; }

        public MonitorCommandBarViewModel(IMonitorCommandBarInteractionListener monitorCommandBarInteractionListener)
        {
            _monitorCommandBarInteractionListener = monitorCommandBarInteractionListener;

            _isStartCommandPressed = false;

            StartCommand = new RelayCommand(async () => await StartCommandHandler(), () => !_isStartCommandPressed);
            StopCommand = new RelayCommand(StopCommandHandler, () => _isStartCommandPressed);
            EditCommand = new RelayCommand(EditCommandHandler, () => !_isStartCommandPressed);
            DeleteCommand = new RelayCommand(async () => await DeleteCommandHandler(), () => !_isStartCommandPressed);
        }

        private void RaiseAll()
        {
            StartCommand.RaiseCanExecuteChanged();
            StopCommand.RaiseCanExecuteChanged();
            EditCommand.RaiseCanExecuteChanged();
            DeleteCommand.RaiseCanExecuteChanged();
        }

        private async Task StartCommandHandler()
        {
            _isStartCommandPressed = true;
            RaiseAll();

            await _monitorCommandBarInteractionListener.OnStartedAsync();

            _isStartCommandPressed = false;
            RaiseAll();
        }

        private void StopCommandHandler()
        {
            _monitorCommandBarInteractionListener.OnStoped();

            _isStartCommandPressed = false;

            RaiseAll();
        }

        private void EditCommandHandler()
        {
            _monitorCommandBarInteractionListener.OnEdited();
        }

        private async Task DeleteCommandHandler()
        {
            await _monitorCommandBarInteractionListener.OnDeletedAsync();
        }
    }
}
