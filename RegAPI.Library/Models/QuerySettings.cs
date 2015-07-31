using System;
using Newtonsoft.Json;
using RegAPI.Library.Models.Interfaces;

namespace RegAPI.Library.Models
{
    /// <summary>
    /// Валюта цен
    /// </summary>
    public enum CurrencyType { USD, EUR, RUB }

	public sealed partial class QuerySettings : IQuerySettings
    {
        [JsonProperty("input_data")]
        public string InputData { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
