using Library.Framework.Core.Extensions;
using log4net;
using log4net.Config;
using log4net.Repository;
using System;

namespace Library.Framework.Core.Plugin
{
    public class NoControllerBase
    {
       
        /// <summary>
        /// 优先级：数字越小优先级越高
        /// </summary>
        public virtual int Priority { get; set; }

        /// <summary>
        /// 插件名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 插件id
        /// </summary>
        public virtual string Id { get; set; }

        protected ILog log;

        protected virtual void Config()
        {
            var config = ConfigurationManage.GetConfiguration($"configuration:6D5AD3FC-EB8E-403E-8C2A-426E87FA7CFA");
            if (config == null)
            {
                throw new Exception("日志配置未找到！");
            }
            ILoggerRepository repository = LogManager.CreateRepository("SysLogger");
            XmlConfigurator.Configure(repository, config.ToStream());
            log = LogManager.GetLogger(repository.Name, "SysLogger");
        }
    }
}
