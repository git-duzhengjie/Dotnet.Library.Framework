using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Framework.Core.Model
{
    public class ConfigurationModel
    {
        /// <summary>
        /// 运行时环境变量
        /// </summary>
        public string Runtime { get; set; }

        /// <summary>
        /// 配置内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}
