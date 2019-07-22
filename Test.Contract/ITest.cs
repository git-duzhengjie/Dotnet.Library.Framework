using Library.Framework.Core.Rpc;
using Microsoft.AspNetCore.Mvc;

namespace Test.Contract
{
    public interface ITest:RpcApi
    {
       HelloDto GetMessage(HelloDto hello);

    }
}
