using Newtonsoft.Json;

namespace RegAPI.Library.Models.Domain
{
    public sealed class DomainAnswer
    {
        [JsonProperty("bill_id")]
        public int BillId { get; set; }

        public Domain[] Domains { get; set; }

        [JsonProperty("pay_notes")]
        public string PayNotes { get; set; }

        [JsonProperty("pay_type")]
        public string PayType { get; set; }

        public int Payment { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
