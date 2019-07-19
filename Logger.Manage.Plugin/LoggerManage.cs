using Library.Framework.Core.Plugin;
using Library.Framework.Core.Utility;
using log4net;
using log4net.Config;
using log4net.Repository;
using System;
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
            ILoggerRepository repository = LogManager.CreateRepository("SysLogger");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
            ILog log = LogManager.GetLogger(repository.Name, "SysLogger");
            SingletonUtility.AddSingleton(log);
        }
    }
}
