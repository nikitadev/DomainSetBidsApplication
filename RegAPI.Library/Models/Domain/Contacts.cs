using System;
using Newtonsoft.Json;

namespace RegAPI.Library.Models.Domain
{
    /// <summary>
    /// контактные данные
    /// </summary>
    public sealed class Contacts
    {
        [JsonProperty("descr")]
        public string Description { get; set; }

        public string Person { get; set; }

        [JsonProperty("person_r")]
        public string PersonLocalName { get; set; }

        [JsonProperty("passport")]
        public string PassportContent { get; set; }

        [JsonProperty("birth_date")]
        public DateTime BirthDate { get; set; }

        [JsonProperty("p_addr")]
        public string PersonAddress { get; set; }

        public string Phone { get; set; }

        [JsonProperty("e_mail")]
        public string Email { get; set; }

        public string Country { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
