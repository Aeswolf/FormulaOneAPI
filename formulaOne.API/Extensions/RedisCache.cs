using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Distributed;

namespace formulaOne.API.Extensions;

public static class RedisCache
{
    static JsonSerializerOptions serializeOptions = new()
    {
        ReferenceHandler = ReferenceHandler.Preserve
    };
    public static async Task SetRecordAsync<T>(
        this IDistributedCache distributedCache,
        string recordKey, T data,
        TimeSpan? absoluteExpirationTime = null, TimeSpan? slidingTime = null
    )
    {
        DistributedCacheEntryOptions cacheEntryOptions = new()
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = slidingTime
        };

        string jsonSerializedData = JsonSerializer.Serialize(data, serializeOptions);

        await distributedCache.SetStringAsync(recordKey, jsonSerializedData, cacheEntryOptions);
    }

    public static async Task<T?> GetRecordAsync<T>(this IDistributedCache distributedCache, string recordKey)
    {
        string? queriedData = await distributedCache.GetStringAsync(recordKey);

        if (queriedData is null) return default(T);

        T? deserializedData = JsonSerializer.Deserialize<T>(queriedData, serializeOptions);

        return deserializedData;
    }
}