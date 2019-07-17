using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Library.Framework.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitExtensions.LoadAssmbleys();
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
