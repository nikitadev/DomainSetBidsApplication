using Newtonsoft.Json;

namespace RegAPI.Library.Models
{
    public class Error
    {
        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }

        [JsonProperty("error_params")]
        public dynamic ErrorParams { get; set; }

        [JsonProperty("error_text")]
        public string ErrorText { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
