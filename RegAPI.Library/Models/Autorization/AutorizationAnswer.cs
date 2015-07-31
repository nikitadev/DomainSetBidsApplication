using Newtonsoft.Json;

namespace RegAPI.Library.Models.Autorization
{
    public sealed class AutorizationAnswer
    {
        public string Login { get; set; }

        [JsonProperty("user_id")]
        public int UserId { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
