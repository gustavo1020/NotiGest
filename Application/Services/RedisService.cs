
using Application.Contracts;
using StackExchange.Redis;
using System.Text.Json;

namespace Application.Services
{
    public class RedisService<T> : IRedisService<T> where T : class
    {
        private readonly IConnectionMultiplexer _redisConnection;
        public RedisService(IConnectionMultiplexer redisConnection) 
        { 
            this._redisConnection = redisConnection;
        }
        public async Task<IEnumerable<T>> SerchCache(string key)
        {
            var database = _redisConnection.GetDatabase();

            var listaDesdeRedis = await database.StringGetAsync(key);

            var listaRecuperada = JsonSerializer.Deserialize<List<T>>(listaDesdeRedis.ToString()) ?? new List<T> { };

            return listaRecuperada;
        }

        public async Task SaveCache(string key, IEnumerable<T> values)
        {
            var database = _redisConnection.GetDatabase();

            var listJson =  JsonSerializer.Serialize(values);

            await database.StringSetAsync(key, listJson);
        }
    }
}
