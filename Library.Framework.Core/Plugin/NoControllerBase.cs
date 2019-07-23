namespace Library.Framework.Core.Plugin
{
    public class NoControllerBase
    {
       
        /// <summary>
        /// 优先级：数字越小优先级越高
        /// </summary>
        public virtual int Priority { get; set; }

        /// <summary>
        /// 插件名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 插件id
        /// </summary>
        public virtual string Id { get; set; }
    }
}
