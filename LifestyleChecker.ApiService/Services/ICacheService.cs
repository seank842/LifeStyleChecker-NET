namespace LifestyleChecker.ApiService.Services
{
    /// <summary>
    /// Defines methods for storing, retrieving, and removing cached data.
    /// </summary>
    /// <remarks>Implementations of this interface provide asynchronous operations to interact with a cache,
    /// allowing for the storage and retrieval of data using a key-value mechanism. The cache can optionally set
    /// expiration times for stored items.</remarks>
    public interface ICacheService
    {
        /// <summary>
        /// Asynchronously sets a value in the cache with the specified key and optional expiration time.
        /// </summary>
        /// <typeparam name="T">The type of the value to be stored in the cache.</typeparam>
        /// <param name="key">The unique key associated with the value in the cache. Cannot be null or empty.</param>
        /// <param name="value">The value to be stored in the cache. Cannot be null.</param>
        /// <param name="expiry">An optional expiration time for the cache entry. If null, the entry does not expire.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// Asynchronously retrieves an object of the specified type from the data store using the provided key.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <param name="key">The unique key identifying the object in the data store. Cannot be null or empty.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the object of type <typeparamref
        /// name="T"/> if found; otherwise, <see langword="null"/>.</returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Asynchronously removes the item associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the item to remove. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous remove operation.</returns>
        Task RemoveAsync(string key);
    }
}
