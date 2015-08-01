using Newtonsoft.Json;

namespace RegAPI.Library.Models.Domain
{
    public sealed class SetReregBidsInputData
    {
        public Contacts Contacts { get; set; }

        public Domain[] Domains { get; set; }

        [JsonProperty("nss")]
        public NSServer NSServer { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
