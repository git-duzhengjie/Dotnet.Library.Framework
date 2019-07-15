using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Framework.Core.Model
{
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
