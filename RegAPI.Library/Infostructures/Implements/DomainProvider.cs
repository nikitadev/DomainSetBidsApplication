using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RegAPI.Library.Infostructures.Interfaces;
using RegAPI.Library.Models;
using RegAPI.Library.Models.Domain;
using RegAPI.Library.Models.Infostructures;
using RegAPI.Library.Models.Interfaces;

namespace RegAPI.Library.Infostructures.Implements
{
    public sealed class DomainProvider : BaseApiProvider, IDomainProvider
    {
        public DomainProvider(IRequestManager requestManager) 
            : base(requestManager)
        {
        }

        protected override UriBuilder GetBaseUri()
        {
            return new UriBuilder(GeneralSettings.ShemaName, GeneralSettings.API_URI, -1, DomainProviderSettings.DOMAIN);
        }

        public async Task<Result<DomainAnswer>> SetReregBidsAsync(string username, string password, SetReregBidsInputData inputData)
        {
            var querySettings = new QuerySettings
            {
                UserName = username,
                Password = password,
                InputData = JsonConvert.SerializeObject(inputData)
            };

            string settings = querySettings.ToString();
            var fullURI = CreateUri(DomainProviderSettings.SETREREGBIDS, settings);

            var json = await _requestManager.Get(fullURI);
            var result = JsonConvert.DeserializeObject<Result<DomainAnswer>>(json);

            return result;
        }
    }
}
