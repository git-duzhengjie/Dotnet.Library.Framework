using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Library.Framework.Web
{
    public static class InitExtensions
    {
        private static string _baseDir=> AppDomain.CurrentDomain.BaseDirectory;
        public static void LoadPlugin(this IServiceCollection services) {
            var pluginDir = Path.Combine(_baseDir, "Plugins");
            foreach (string dll in Directory.GetFiles(pluginDir, "*.dll")) {
                try
                {
                    services.AddMvc().AddApplicationPart(AssemblyLoadContext.Default.LoadFromAssemblyPath(dll));
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void LoadDoc(this SwaggerGenOptions options) {
            var dir = Path.Combine(_baseDir, "Doc");
            var din = new DirectoryInfo(dir);
            foreach (var d in din.GetFiles("*.xml", SearchOption.AllDirectories)) {
                options.IncludeXmlComments(d.FullName);
            }
        }

        public static void LoadAssmbleys()
        {
            var dir = Path.Combine(_baseDir, "DynamicLib");
            DirectoryInfo dynamicInfo = new DirectoryInfo(dir);
            if (dynamicInfo.Exists)
            {
                var fileInfoList = dynamicInfo.GetFiles("*.dll");
                foreach (var fileInfo in fileInfoList)
                {
                    using (FileStream fileStream = fileInfo.OpenRead())
                    {
                        var resourceBytes = new byte[fileInfo.Length];
                        fileStream.Read(resourceBytes, 0, resourceBytes.Length);
                        var dependAssembly = Assembly.Load(resourceBytes);
                    }
                }
            }

        }
    }
}
