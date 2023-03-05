using StackExchange.Redis;

namespace MarketPlace
{
    public static class RedisStore
    {
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection = new(() => ConnectionMultiplexer.Connect(new ConfigurationOptions
        {
            EndPoints = { "localhost:6379" }
        }));
        public static ConnectionMultiplexer Connection => LazyConnection.Value;
        public static IDatabase RedisCashe => Connection.GetDatabase();
    }
}
