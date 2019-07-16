using Library.Framework.Core.Utility;
using System;
using Test.Contract;

namespace Test.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = MqRpcHelper.CreateType<ITest>();
            t.GetMessage(new HelloDto {
            User="杜政颉",
            Content="hello world!"});
            var r = t.GetMessage(new HelloDto { User = "duzhengjie", Content = "hello" });
            Console.WriteLine($"{r.User}:{r.Content}");
        }
    }
}
