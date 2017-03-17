using StackExchange.Redis;

namespace backendApp
{
    public class RedisClient
    {
        private const string HOST = "localhost:6379";
        private static ConnectionMultiplexer connection;

        static RedisClient()
        {
            connection = ConnectionMultiplexer.Connect(HOST);
        }

        public static IDatabase getDb() 
        {
            return connection.GetDatabase();
        }
    }
}
