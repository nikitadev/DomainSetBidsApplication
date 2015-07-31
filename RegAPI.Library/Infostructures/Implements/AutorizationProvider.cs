using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RegAPI.Library.Infostructures.Interfaces;
using RegAPI.Library.Models;
using RegAPI.Library.Models.Autorization;
using RegAPI.Library.Models.Infostructures;
using RegAPI.Library.Models.Interfaces;

namespace RegAPI.Library.Infostructures.Implements
{
    public sealed class AutorizationProvider : BaseApiProvider, IAutorizationProvider
    {
        public AutorizationProvider(IRequestManager requestManager) : base(requestManager)
        {
        }

        public async Task<Result<AutorizationAnswer>> CheckAsync(string username, string password)
        {
            var querySettings = new QuerySettings
            {
                UserName = username,
                Password = password
            };

            string settings = querySettings.ToString();
            var fullURI = CreateUri(GeneralSettings.NOP, settings);

            var json = await _requestManager.Get(fullURI);
            var result = JsonConvert.DeserializeObject<Result<AutorizationAnswer>>(json);

            return result;
        }

        public async Task<Result<AutorizationAnswer>> GetUserIdAsync(string username, string password)
        {
            var querySettings = new QuerySettings
            {
                UserName = username,
                Password = password
            };

            string settings = querySettings.ToString();
            var fullURI = CreateUri(AutorizationProviderSettings.GETUSERID, settings);

            var json = await _requestManager.Get(fullURI);
            var result = JsonConvert.DeserializeObject<Result<AutorizationAnswer>>(json);

            return result;
        }

        protected override UriBuilder GetBaseUri()
        {
            return new UriBuilder(GeneralSettings.ShemaName, GeneralSettings.API_URI, -1);
        }
    }
}
