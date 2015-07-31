using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RegAPI.Library.Models.Domain
{
    /// <summary>
    /// Информация по домену
    /// </summary>
    public sealed class Domain
    {
        [JsonProperty("dname")]
        public string Name { get; set; }

        public decimal Price { get; set; }

        [JsonProperty("service_id")]
        public int? ServiceId { get; set; }
        
        public string Result { get; set; }
    }
}
