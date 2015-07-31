using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RegAPI.Library.Models.Domain
{
    public sealed class SetReregBidsInputData
    {
        public Contacts Contacts { get; set; }

        [JsonProperty("nss")]
        public NSServer NSServer { get; set; }

        public Domain[] Domains { get; set; }
    }
}
