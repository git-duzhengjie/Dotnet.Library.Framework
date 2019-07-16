using System;
namespace Test.Contract
{
    [System.Serializable]
    public class HelloDto
    {
        /// <summary>
        /// 用户
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
