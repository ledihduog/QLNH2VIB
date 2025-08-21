namespace QuanLyNhaHang.IRepository
{
    public interface IResponseCacheRepository
    {
        Task<string> GetCachResponseAsync(string cacheKey);
        Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task RemoveCacheResponseAsync(string cacheKey);
        Task RemoveCache();
    }
}
