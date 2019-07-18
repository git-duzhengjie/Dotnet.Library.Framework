using Library.Framework.Core.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Framework.Core.Model
{
    public class DatabaseConfiguration
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public DBType DBtype { get; set; }
        /// <summary>
        /// 是否自己录入连接字符串
        /// </summary>
        public bool IsConnectionStr { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionStr { get; set; }
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
        /// <summary>
        /// 连接池
        /// </summary>
        public bool Pooling { get; set; }
        /// <summary>
        /// 连接池中允许的最大连接数
        /// </summary>
        public int MaxPoolSize { get; set; }
        /// <summary>
        /// 连接池中允许的最小连接数
        /// </summary>
        public int MinPoolSize { get; set; }

        public string Connection
        {
            get
            {
                if (IsConnectionStr)
                    return ConnectionStr;
                else
                {
                    string pool = Pooling ? $"pooling={Pooling};maxpoolsize={MaxPoolSize};minpoolsize={MinPoolSize}" : "";
                    return $"server={Host};uid={User};pwd={Password};database={Database};port={Port};{pool}";
                }
            }
        }


    }
}
