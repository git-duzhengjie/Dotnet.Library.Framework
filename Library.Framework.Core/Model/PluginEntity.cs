using System.Reflection;

namespace Library.Framework.Web.Model
{
    public class PluginEntity
    {
        /// <summary>
        /// 插件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 插件id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 插件优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 插件程序集
        /// </summary>
        public Assembly Assembly { get; set; }
    }
}
