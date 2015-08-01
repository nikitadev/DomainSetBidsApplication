using DomainSetBidsApplication.Fundamentals.Interfaces;
using Newtonsoft.Json;

namespace DomainSetBidsApplication.Models
{
    public static class DbEntity
    {
        public static string ToJson(this IDbEntity entity)
        {
            return JsonConvert.SerializeObject(entity);
        }
    }
}
