using Library.Framework.Core.Plugin;
using Library.Framework.Core.Utility;

namespace Cache.Manage.Plugin
{
    public class CacheManage:NoControllerBase
    {
        /// <summary>
        /// 插件ID
        /// </summary>
        public override string Id => "A3CCB219-379C-4AE6-A672-85E835498E90";

        /// <summary>
        /// 插件名
        /// </summary>
        public override string Name => "缓存";


        public override int Priority => -10;

        public CacheManage() {
            RedisHelper redis = new RedisHelper("192.168.137.2:6379,ssl=false,connectTimeout=5000");
            SingletonUtility.AddSingleton(redis);
        }
    }
}
