using Library.Framework.Core.Utility;
using System;
using System.Reflection;
using Test.Contract;

namespace Test.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Test t = new Test();
            MqRpcHelper.RegisterRpcServer(typeof(ITest).FullName, (r) => {
                var c = r.Content;
                Console.WriteLine(r.Content.Method);
                MethodInfo m = typeof(Test).GetMethod(c.Method);
                if (m != null)
                    return m.Invoke(t, c.Params);
                return null;
            });
        }
    }
}
