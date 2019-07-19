using Library.Framework.Core.Aspnet;
using Library.Framework.Core.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Test.Contract;

namespace Library.Framework.Plugin
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    public class TestPlugin : WebApiController,ITest
    {

        /// <summary>
        /// 
        /// </summary>
        public override bool IsAuth => false;

        /// <summary>
        /// 
        /// </summary>
        public override bool IsRegisterRpc => true;

        /// <summary>
        /// 
        /// </summary>
        public override string Name => "测试";
        /// <summary>
        /// 
        /// </summary>
        public override string Id => "B95B6E07-EE83-4458-83CA-0843D3F0C7E9";
        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hello"></param>
        /// <returns></returns>
        [HttpPost("getMessage")]
        public HelloDto GetMessage([FromBody]HelloDto hello)
        {
            var cache = SingletonUtility.GetSingleton<RedisHelper>();
            return hello;
        }
    }
}
