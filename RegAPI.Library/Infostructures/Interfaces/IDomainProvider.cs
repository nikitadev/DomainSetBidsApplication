using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using RegAPI.Library.Models;
using RegAPI.Library.Models.Domain;

namespace RegAPI.Library.Infostructures.Interfaces
{
    /// <summary>
    /// Провайдер для работы с доменами
    /// </summary>
    [ContractClass(typeof(DomainProviderContract))]
    public interface IDomainProvider
    {
        Task<Result<DomainAnswer>> SetReregBidsAsync(string username, string password, SetReregBidsInputData inputData);
    }

    [ContractClassFor(typeof(IDomainProvider))]
    internal abstract class DomainProviderContract : IDomainProvider
    {
        public Task<Result<DomainAnswer>> SetReregBidsAsync(string username, string password, SetReregBidsInputData inputData)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(username), "Username can not be null or empty.");
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(password), "Password can not be null or empty.");
            Contract.Requires<ArgumentNullException>(inputData != null, "Input Data can not be null or empty.");

            return Task.FromResult(default(Result<DomainAnswer>));
        }
    }
}
