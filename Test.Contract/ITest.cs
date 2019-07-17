using Microsoft.AspNetCore.Mvc;

namespace Test.Contract
{
    public interface ITest
    {
        ActionResult<HelloDto> GetMessage(HelloDto hello);

    }
}
