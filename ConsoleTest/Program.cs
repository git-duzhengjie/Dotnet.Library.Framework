using RabbitMq.Manage.Plugin;
using System;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("Runtime", "Dev");
            new RabbitMqManage();
            Console.Read();
        }
    }
}
