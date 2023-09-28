using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace formulaOne.API.Extensions;

public static class RedisCache
{
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

        string jsonSerializedData = JsonSerializer.Serialize(data);

        await distributedCache.SetStringAsync(recordKey, jsonSerializedData, cacheEntryOptions);
    }

    public static async Task<T?> GetRecordAsync<T>(this IDistributedCache distributedCache, string recordKey)
    {
        string? queriedData = await distributedCache.GetStringAsync(recordKey);

        if (queriedData is null) return default(T);

        T? deserializedData = JsonSerializer.Deserialize<T>(queriedData);

        return deserializedData;
    }
}