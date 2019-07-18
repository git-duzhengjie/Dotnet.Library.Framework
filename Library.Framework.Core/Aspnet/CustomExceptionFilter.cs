using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Library.Framework.Core.Aspnet
{
    public class CustomExceptionFilter : Attribute, IExceptionFilter
    {
        public CustomExceptionFilter()
        {
        }

        public void OnException(ExceptionContext context)
        {
            //Exception exception = context.Exception;
            //string error = string.Empty;

            //void ReadException(Exception ex)
            //{
            //    error += string.Format("{0} | {1} | {2}", ex.Message, ex.StackTrace, ex.InnerException);
            //    if (ex.InnerException != null)
            //    {
            //        ReadException(ex.InnerException);
            //    }
            //}

            //ReadException(context.Exception);

            if (context.Exception.Message == "非法的请求！")
            {

                ContentResult result = new ContentResult
                {
                    StatusCode = 200,
                    ContentType = "text/json;charset=utf-8;",
                    Content = "{\"code\":401,\"message\":\"非法的请求！\"}"
                };
                context.Result = result;
                context.ExceptionHandled = true;
            }
        }
    }
}
