using System.Threading.Tasks;

namespace DomainSetBidsApplication.ViewModels.InteractionListeners
{
    public interface IMonitorCommandBarInteractionListener
    {
        Task OnStartedAsync();
        void OnStoped();
        void OnEdited();
        Task OnDeletedAsync();
    }
}
