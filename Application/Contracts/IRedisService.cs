namespace Application.Contracts
{
    public interface IRedisService<T> where T : class
    {
        Task<IEnumerable<T>> SerchCache(string key);
        Task SaveCache(string key,  IEnumerable<T> values);
    }
}
