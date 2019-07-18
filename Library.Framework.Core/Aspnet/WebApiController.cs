using Library.Framework.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.IO;

namespace Library.Framework.Core.Aspnet
{
    public class WebApiController : Controller
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
        public int Priority { get; set; }

        /// <summary>
        /// 插件名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 插件id
        /// </summary>
        public virtual string Id { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            if (IsAuth && !Request.Headers.ContainsKey("token"))
            {
                context.HttpContext.Response.StatusCode = 401;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if (IsAuth && !Request.Headers.ContainsKey("token"))
            {
                context.HttpContext.Response.Body = new MemoryStream();
            }
            //
            //
            //wr.Write("hello");
            //=stream;
        }

    }
}
