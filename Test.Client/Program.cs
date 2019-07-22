using Library.Framework.Core.Utility;
using RabbitMq.Manage.Plugin;
using System;
using Test.Contract;

namespace Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("Runtime", "Dev");
            new RabbitMqManage();
            var t = MqRpcHelper.CreateType<ITest>();
            var r = t.GetMessage(new HelloDto { User = "duzhengjie", Content = "hello" });
            Console.WriteLine($"{r.User}:{r.Content}");
        }
    }
}
