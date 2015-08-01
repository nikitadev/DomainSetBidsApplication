using Newtonsoft.Json.Serialization;

namespace RegAPI.Library
{
    internal sealed class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
