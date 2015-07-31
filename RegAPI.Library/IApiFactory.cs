using System;
using System.Diagnostics.Contracts;
using RegAPI.Library.Infostructures.Interfaces;

namespace RegAPI.Library
{
    [ContractClass(typeof(ApiFactoryContract))]
    public interface IApiFactory
    {
        IDomainProvider Domain { get; }

        IAutorizationProvider Autorization { get; }
    }

    [ContractClassFor(typeof(IApiFactory))]
    internal abstract class ApiFactoryContract : IApiFactory
    {
        public IAutorizationProvider Autorization
        {
            get
            {
                Contract.Ensures(Contract.Result<IAutorizationProvider>() != null);

                return default(IAutorizationProvider);
            }
        }

        public IDomainProvider Domain
        {
            get
            {
                Contract.Ensures(Contract.Result<IDomainProvider>() != null);

                return default(IDomainProvider);
            }
        }
    }
}
