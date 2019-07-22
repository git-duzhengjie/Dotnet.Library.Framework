using Library.Framework.Core.Extensions;
using Library.Framework.Core.Model;
using Library.Framework.Core.Plugin;
using Library.Framework.Core.Utility;
using log4net;
using log4net.Config;
using log4net.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Logger.Manage.Plugin
{
    public class LoggerManage:NoControllerBase
    {
        /// <summary>
        /// 插件id
        /// </summary>
        public override string Id => "6D5AD3FC-EB8E-403E-8C2A-426E87FA7CFA";

        /// <summary>
        /// 插件名
        /// </summary>
        public override string Name => "日志";

        /// <summary>
        /// 插件优先级
        /// </summary>
        public override int Priority => -11;

        public LoggerManage() {
            var config = ConfigurationManage.GetConfiguration($"configuration:{Id}");
            if (config == null) {
                var cg = new ConfigurationModel
                {
                    Content= File.ReadAllText("log4net.config"),
                    Runtime=Environment.GetEnvironmentVariable("Runtime"),
                    Description="日志"
                };
                IList<ConfigurationModel> cgl = new List<ConfigurationModel>();
                cgl.Add(cg);
                config = cg.Content;
                ConfigurationManage.SetConfiguration($"configuration:{Id}", cgl);
            }
            ILoggerRepository repository = LogManager.CreateRepository("SysLogger");
            XmlConfigurator.Configure(repository, config.ToStream());
            ILog log = LogManager.GetLogger(repository.Name, "SysLogger");
            SingletonUtility.AddSingleton(log);
        }
    }
}
