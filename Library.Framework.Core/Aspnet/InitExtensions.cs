﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Linq;
using System.Collections.Generic;
using Library.Framework.Core.Utility;
using Library.Framework.Core.Model;
using Library.Framework.Web.Model;
using Swashbuckle.AspNetCore.SwaggerGen;
using Library.Framework.Core.Aspnet;
using Library.Framework.Core.Plugin;

namespace Library.Framework.Web
{
    public static class InitExtensions
    {
        private static string _baseDir=> AppDomain.CurrentDomain.BaseDirectory;
        public static void LoadPlugin(this IServiceCollection services) {
            var pluginDir = Path.Combine(_baseDir, "Plugins");
            if (!Directory.Exists(pluginDir))
                return;
            Console.ForegroundColor = ConsoleColor.Green;
            IList<PluginEntity> plugins = new List<PluginEntity>();
            var dlls = Directory.GetFiles(pluginDir, "*.dll");
            foreach (string dll in dlls) {
                Type type=null;
                try
                {
                    var assemb = AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                    var types = assemb.GetTypes();
                    var query = from t in types where t.IsClass && 
                                (t.BaseType == typeof(WebApiController)||t.BaseType==typeof(NoControllerBase)) select t;
                    type = query.ToList().First();
                    PluginEntity plugin=null;
                    if (type == null)
                    {
                        continue;
                    }
                    else
                    {
                        var obj = Activator.CreateInstance(type);
                        var priority = type.GetProperty("Priority").GetValue(obj);
                        var name = type.GetProperty("Name").GetValue(obj);
                        var id = type.GetProperty("Id").GetValue(obj);
                        bool isAuth=false;
                        if(type.BaseType==typeof(WebApiController))
                            isAuth = (bool)type.GetProperty("IsAuth").GetValue(obj);
                        plugin = new PluginEntity
                        {
                            Name = (string)name,
                            Id = (string)id,
                            Priority = (int)priority,
                            Assembly = assemb,
                            IsAuth = isAuth,
                            Type = type,
                            Obj=obj
                        };
                    }
                    plugins.Add(plugin);
                }
                catch (Exception ex) {
                    Console.ResetColor();
                    Console.WriteLine(type?.FullName+ex.ToString());
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            var sort = from k in plugins orderby k.Priority ascending select k; 
            foreach (var k in sort) {
                if (k.Type.BaseType == typeof(WebApiController))
                {
                    services.AddMvcCore().AddApplicationPart(k.Assembly);
                    MethodInfo m1 = k.Type.GetMethod("Config");
                    m1.Invoke(k.Obj, new object[] { });
                }
                Console.WriteLine($"【{k.Priority}】{k.Name}插件（{k.Id}）加载成功!");
                if(k.IsAuth)
                    Console.WriteLine($"【{k.Priority}】{k.Name}插件（{k.Id}）开启token验证!");
                var interfaces = k.Type.GetInterfaces();
                if (interfaces == null || interfaces.ToList().Count == 0)
                    continue;
                var contract = (from t in interfaces where t.GetInterface("RpcApi") != null select t.FullName).FirstOrDefault();
                if (contract == null)
                    continue;
                MqRpcHelper.RegisterRpcServer(contract, (r) =>
                {
                    var c = r.Content;
                    MethodInfo m = k.Type.GetMethod(c.Method);
                    if (m != null)
                        return m.Invoke(k.Obj, c.Params);
                    return DResult.Error("Rpc服务中心没有找到该方法！");
                });

                Console.WriteLine($"【{k.Priority}】{k.Name}插件（{k.Id}）rpc注册成功!");
            }
            Console.ResetColor();
        }
        public static void LoadLib() {
            var libDir = Path.Combine(_baseDir, "Lib");
            if (!Directory.Exists(libDir))
                return;
            foreach (string dll in Directory.GetFiles(libDir, "*.dll"))
            {
                try
                {
                    AssemblyLoadContext.Default.LoadFromAssemblyPath(dll);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("载入依赖库报错！",ex.ToString());
                    Console.ResetColor();
                }
            }
        }
        public static void LoadDoc(this SwaggerGenOptions options) {
            var dir = Path.Combine(_baseDir, "Doc");
            if (!Directory.Exists(dir))
                return;
            var din = new DirectoryInfo(dir);
            foreach (var d in din.GetFiles("*.xml", SearchOption.AllDirectories)) {
                options.IncludeXmlComments(d.FullName);
            }
        }

        public static void LoadPlugin<T>(this IServiceCollection services, T _plugin) where T:WebApiController
        {
            Console.ForegroundColor = ConsoleColor.Green;
            var plugin = (WebApiController)_plugin;
            var type = typeof(T);
            var interfaces = type.GetInterfaces();
            if (interfaces != null && interfaces.ToList().Count != 0)
            {
                var contract = (from t in interfaces where t.GetInterface("RpcApi") != null select t.FullName).FirstOrDefault();
                if (contract != null)
                {
                    var obj = Activator.CreateInstance(type);
                    MqRpcHelper.RegisterRpcServer(contract, (r) =>
                    {
                        var c = r.Content;
                        MethodInfo m = type.GetMethod(c.Method);
                        if (m != null)
                            return m.Invoke(obj, c.Params);
                        return DResult.Error("Rpc服务中心没有找到该方法！");
                    });

                    Console.WriteLine($"【{plugin.Priority}】{plugin.Name}插件（{plugin.Id}）rpc注册成功!");
                }
            }
            if (type.BaseType == typeof(WebApiController))
            {
                services.AddMvcCore().AddApplicationPart(type.Assembly);
                MethodInfo m1 = type.GetMethod("Config");
                var obj = Activator.CreateInstance(type);
                m1.Invoke(obj, new object[] { });
            }
            
            Console.WriteLine($"【{plugin.Priority}】{plugin.Name}插件（{plugin.Id}）加载成功!");
            if (plugin.IsAuth)
                Console.WriteLine($"【{plugin.Priority}】{plugin.Name}插件（{plugin.Id}）开启token验证!");
            Console.ResetColor();
        }
    }
}
