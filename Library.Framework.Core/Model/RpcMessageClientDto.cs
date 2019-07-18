using System;
namespace Library.Framework.Core.Model
{
    [System.Serializable]
    public class RpcMessageClientDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object[] Params { get; set; }
    }
}
