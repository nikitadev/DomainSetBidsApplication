using System.Threading;
using System.Threading.Tasks;
using DomainSetBidsApplication.Models;
using GalaSoft.MvvmLight;

namespace DomainSetBidsApplication.ViewModels
{
    public sealed class RegDomainViewModel : ViewModelBase
    {
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

        public Task Task { get; set; }

        public CancellationTokenSource TokenSource { get; set; }
    }
}
