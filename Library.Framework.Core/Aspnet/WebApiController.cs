using Microsoft.AspNetCore.Mvc;

namespace Library.Framework.Core.Aspnet
{
    public class WebApiController : ControllerBase
    {
        /// <summary>
        /// 是否开启验证
        /// </summary>
        public virtual bool IsAuth { get; set; }

        /// <summary>
        /// 是否注册rpc服务
        /// </summary>
        public virtual bool IsRegisterRpc { get; set; }
        /// <summary>
        /// 优先级：数字越小优先级越高
        /// </summary>
        public int priority { get; set; }
    }
}
