using System;
using System.Threading;
using System.Threading.Tasks;
using DomainSetBidsApplication.Models;

namespace DomainSetBidsApplication.ViewModels.InteractionListeners
{
    public interface IRegDomainInteractionListener
    {
        Task<Tuple<Task, CancellationTokenSource>> GetTaskAsync(RegDomainEntity entity);

        Task OnDomainRemoveAsync(RegDomainEntity entity);
    }
}
