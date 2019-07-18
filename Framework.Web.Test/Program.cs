using Library.Framework.Web;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Framework.Web.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitExtensions.LoadLib();
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) {
            var urls = string.Format("http://*:{0}/", 9099);
            return WebHost.CreateDefaultBuilder(args)
                .UseUrls(urls)
                .UseStartup<Startup>();
        }
            
            
    }
}
