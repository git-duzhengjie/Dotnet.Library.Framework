﻿using Library.Framework.Core.Extensions;
using StackExchange.Redis;
using System;

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
        public bool SetValue(string key, string value, int ?seconds=null)
        {
            if(seconds!=null)
                return Db.StringSet(key, value, TimeSpan.FromSeconds((double)seconds));
            return Db.StringSet(key, value);
        }

        public bool SetValue<T>(string key, T value, int? seconds = null)
        {
            if(seconds==null)
                return Db.StringSet(key, value.SerializeJson());
            return Db.StringSet(key, value.SerializeJson(), TimeSpan.FromSeconds((double)seconds));
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
            var v = Db.StringGet(key).ToString();
            if (v.IsNullOrEmpty()) {
                return default;
            }
            return v.DeserializeJson<T>();
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
