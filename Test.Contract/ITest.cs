using Microsoft.AspNetCore.Mvc;

namespace Test.Contract
{
    public interface ITest
    {
       HelloDto GetMessage(HelloDto hello);

    }
}
