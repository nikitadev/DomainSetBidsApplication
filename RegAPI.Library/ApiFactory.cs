using System;
using RegAPI.Library.Infostructures.Implements;
using RegAPI.Library.Infostructures.Interfaces;
using RegAPI.Library.Models.Implements;
using RegAPI.Library.Models.Interfaces;

namespace RegAPI.Library
{
    public sealed class ApiFactory : IApiFactory
    {
        private readonly IRequestManager _requestManager;

        private readonly Func<IDomainProvider> _creatorDomainProvider;
        private readonly Func<IAutorizationProvider> _creatorAutorizationProvider;

        private IDomainProvider _domainProvider;
        public IDomainProvider Domain
        {
            get
            {
                if (_domainProvider == null)
                {
                    _domainProvider = _creatorDomainProvider.Invoke();
                }

                return _domainProvider;
            }
        }

        private IAutorizationProvider _autorizationProvider;
        public IAutorizationProvider Autorization
        {
            get
            {
                if (_autorizationProvider == null)
                {
                    _autorizationProvider = _creatorAutorizationProvider.Invoke();
                }

                return _autorizationProvider;
            }
        }

        public ApiFactory()
        {
            _requestManager = new RequestManager();

            _creatorDomainProvider = new Func<IDomainProvider>(() => new DomainProvider(_requestManager));
            _creatorAutorizationProvider = new Func<IAutorizationProvider>(() => new AutorizationProvider(_requestManager));
        }
    }
}
