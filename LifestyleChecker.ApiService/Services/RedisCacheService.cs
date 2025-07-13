using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LifestyleChecker.ApiService.Services
{
    /// <summary>
    /// Provides methods for interacting with a Redis cache, allowing storage and retrieval of data using string keys.
    /// </summary>
    /// <remarks>This service uses a Redis database to store serialized objects as strings. It supports
    /// asynchronous operations for setting and getting values, with optional expiration times for cache entries. The
    /// service is initialized with a connection to a Redis server, which is used to perform all cache
    /// operations.</remarks>
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheService"/> class using the specified connection
        /// multiplexer.    
        /// </summary>
        /// <param name="connectionMultiplexer">The connection multiplexer used to connect to the Redis server. This parameter cannot be <see
        /// langword="null"/>.</param>
        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _db = connectionMultiplexer.GetDatabase();
        }

        /// <summary>
        /// Asynchronously sets a value in the database with the specified key and optional expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the value to be stored.</typeparam>
        /// <param name="key">The key under which the value will be stored. Cannot be null or empty.</param>
        /// <param name="value">The value to be stored. Must be serializable to JSON.</param>
        /// <param name="expiry">An optional expiration time for the stored value. If null, the value will not expire.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            await _db.StringSetAsync(key, json, expiry);
        }

        /// <summary>
        /// Asynchronously retrieves an object of the specified type from the database using the given key.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="key">The key associated with the object to retrieve. Cannot be null or empty.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the deserialized object of type
        /// <typeparamref name="T"/> if the key exists and the value is not null or empty; otherwise, <see
        /// langword="default"/>.</returns>
        public async Task<T?> GetAsync<T>(string key)
        {
            var json = await _db.StringGetAsync(key);
            if (json.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(json!, _jsonOptions);
        }

        /// <summary>
        /// Asynchronously removes the specified key from the database.
        /// </summary>
        /// <param name="key">The key to be removed. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous remove operation.</returns>
        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    }
}
