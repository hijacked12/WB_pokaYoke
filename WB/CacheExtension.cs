using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace WB
{
    public static class MyCacheService
    {
       

        public static void SetObject<T>(this IDistributedCache _cache, string key, T value)
        {
            //var serializedList = JsonConvert.SerializeObject(list);
            _cache.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObject<T>(this IDistributedCache _cache, string key)
        {
            string data = _cache.GetString(key);
            return data == null ? default : JsonSerializer.Deserialize<T>(data);
        }
    }
}
