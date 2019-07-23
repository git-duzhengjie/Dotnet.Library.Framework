using Library.Framework.Core.Model;
using Library.Framework.Core.Utility;
using System;
using System.Collections.Generic;

namespace Library.Framework.Core.Plugin
{
    public class ConfigurationManage
    {
        private static RedisHelper _redis = new RedisHelper("111.231.220.87:6379,password=gpsgps2019,ssl=false,connectTimeout=5000");
        public static string GetConfiguration(string id) {
            var runtime = Environment.GetEnvironmentVariable("Runtime");
            if (runtime == null)
                throw new Exception("环境变量runtime不存在！");
            IList<ConfigurationModel> configurations = _redis.GetValue<IList<ConfigurationModel>>(id);
            if (configurations == null)
                return null;
            foreach (var config in configurations) {
                if (config.Runtime == null)
                    throw new Exception("配置中心环境变量Runtime不能空！");
                if (config.Runtime.Equals(runtime))
                    return config.Content;
            }
            return null;
        }

        public static void SetConfiguration(string id, IList<ConfigurationModel> configuration) {
            _redis.SetValue(id, configuration);
        }
    }
}
