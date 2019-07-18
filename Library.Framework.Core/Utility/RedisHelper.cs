using Library.Framework.Core.Extensions;
using StackExchange.Redis;

namespace Library.Framework.Core.Utility
{
    public class RedisHelper
    {
        private ConnectionMultiplexer Redis { get; set; }
        private IDatabase Db { get; set; }
        public RedisHelper(string connection)
        {
            Redis = ConnectionMultiplexer.Connect(connection);
            Db = Redis.GetDatabase();
        }

        /// <summary>
        /// 增加/修改
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetValue(string key, string value)
        {
            return Db.StringSet(key, value);
        }

        public bool SetValue<T>(string key, T value)
        {
            return Db.StringSet(key, value.SerializeJson());
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return Db.StringGet(key);
        }

        public T GetValue<T>(string key)
        {
            return Db.StringGet(key).ToString().DeserializeJson<T>();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool DeleteKey(string key)
        {
            return Db.KeyDelete(key);
        }
    }
}
