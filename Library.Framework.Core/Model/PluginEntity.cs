using System;
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

        /// <summary>
        /// 是否开启token校验
        /// </summary>
        public bool IsAuth { get; set; }

        /// <summary>
        /// 是否注册rpc服务
        /// </summary>
        public bool IsRegisterRpc { get; set; }

        /// <summary>
        /// 插件类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 插件实例
        /// </summary>
        public object Obj { get; set; }
    }
}
