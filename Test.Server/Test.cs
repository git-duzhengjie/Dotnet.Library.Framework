using System;
using Test.Contract;

namespace Test.Server
{
    public class Test : ITest
    {
        public HelloDto GetMessage(HelloDto hello)
        {
            return hello;
        }

        public void SayHello(string word)
        {
            Console.WriteLine(word);
        }
    }
}
