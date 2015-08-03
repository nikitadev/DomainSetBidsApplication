using System.Threading.Tasks;

namespace DomainSetBidsApplication.ViewModels.InteractionListeners
{
    public interface IMonitorCommandBarInteractionListener
    {
        Task OnStartedAsync();

        bool CanStarted();
        void OnStoped();
        void OnEdited();
        Task OnDeletedAsync();
    }
}
