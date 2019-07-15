using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Framework.Core.Model
{
    public class DatabaseConfiguration
    {
        /// <summary>
        /// 服务器
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 数据库
        /// </summary>
        public string Database { get; set; }
    }
}
