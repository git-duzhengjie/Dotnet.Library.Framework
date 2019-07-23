using Library.Framework.Core.Extensions;
using Library.Framework.Core.Model;
using Library.Framework.Core.Plugin;
using Library.Framework.Core.Utility;
using System;
using System.Collections.Generic;

namespace RabbitMq.Manage.Plugin
{
    public class RabbitMqManage : NoControllerBase
    {
        public override string Id => "4384375F-2014-4709-96F0-F3E19C5DD91E";

        public override string Name => "rabbitmq";

        public override int Priority => -9;

        public RabbitMqManage() {
            var config = ConfigurationManage.GetConfiguration($"configuration:{Id}");
            Configuration cg = null;
            if (config == null)
            {
                IList<ConfigurationModel> configurations = new List<ConfigurationModel>();
                var configuration = new ConfigurationModel
                {
                    Runtime = Environment.GetEnvironmentVariable("Runtime"),
                    Description = "rabbitmq",
                    Content = new Configuration
                    {
                        ConnectString = "amqp://192.168.137.2:5672/",
                        User = "guest",
                        Password = "guest"
                    }.SerializeJson(),
                };
                configurations.Add(configuration);
                ConfigurationManage.SetConfiguration($"configuration:{Id}", configurations);
                cg = configuration.Content.DeserializeJson<Configuration>();
            }
            else {
                cg = config.DeserializeJson<Configuration>();
            }
            var rabbitMqHelper = new RabbitMqHelper(cg.ConnectString, cg.User, cg.Password);
            SingletonUtility.AddSingleton(rabbitMqHelper);
        }
    }
}
