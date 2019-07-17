using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Library.Framework.Core.Aspnet
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            var cultureQuery = context.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {

            }

            // Call the next delegate/middleware in the pipeline
            return this._next(context);
        }
    }
}
