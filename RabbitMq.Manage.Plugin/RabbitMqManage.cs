using Library.Framework.Core.Plugin;
using Library.Framework.Core.Utility;

namespace RabbitMq.Manage.Plugin
{
    public class RabbitMqManage : NoControllerBase
    {
        public override string Id => "4384375F-2014-4709-96F0-F3E19C5DD91E";

        public override string Name => "rabbitmq";

        public override int Priority => -9;

        public RabbitMqManage() {
            var rabbitMqHelper = new RabbitMqHelper("amqp://192.168.137.2:5672/", "guest", "guest", 2);
            SingletonUtility.AddSingleton(rabbitMqHelper);
        }
    }
}
