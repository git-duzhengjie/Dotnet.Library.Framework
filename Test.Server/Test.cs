using System;
using Microsoft.AspNetCore.Mvc;
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

        HelloDto ITest.GetMessage(HelloDto hello)
        {
            throw new NotImplementedException();
        }
    }
}
