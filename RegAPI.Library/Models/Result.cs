using System;
using Newtonsoft.Json;

namespace RegAPI.Library.Models
{
    public enum ResultType { SUCCESS, ERROR };

    public sealed class Result<T> : Error where T : class
	{
        [JsonProperty("result")]
        public string ResultQuery { get; set; }

        public ResultType ResultType
        {
            get
            {
                ResultType resultType;
                Enum.TryParse(ResultQuery.ToUpper(), out resultType);

                return resultType;
            }
        }

        public T Answer { get; set; }
	}
}
