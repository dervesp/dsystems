using Newtonsoft.Json;
using System;

namespace backendApp
{
    public class UserStorageManager
    {
        private const string KEY_PREFIX = "RegistrationApp";
        private const string KEY_USER_COUNTER = KEY_PREFIX + ".User.Counter";
        private const string KEY_USER_RECORD = KEY_PREFIX + ".User.";

        public static int addUser(User user)
        {
            int userId = getNewUserId();
            RedisClient.getDb().StringSet(KEY_USER_RECORD + userId, JsonConvert.SerializeObject(user));
            return userId;
        }

        public static User getUser(int userId)
        {
            var redisValue = RedisClient.getDb().StringGet(KEY_USER_RECORD + userId);
            User user = null;
            if (redisValue.HasValue)
            {
                try
                {
                    user = JsonConvert.DeserializeObject<User>(redisValue.ToString());
                }
                catch (JsonException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return user;
        }

        private static int getNewUserId()
        {
            int newId;
            var redisValue = RedisClient.getDb().StringGet(KEY_USER_COUNTER);
            if (redisValue.IsNullOrEmpty)
            {
                newId = 0;
                RedisClient.getDb().StringSet(KEY_USER_COUNTER, newId);
            }
            else
            {
                newId = (int)redisValue;
            }
            RedisClient.getDb().StringIncrement(KEY_USER_COUNTER);
            return newId;
        }
    }
}
