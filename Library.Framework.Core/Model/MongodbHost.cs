using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Framework.Core.Model
{
    public class MongodbHost
    {
        public string Connection { get; set; }
        public string DataBase { get; set; }
        public string Table { get; set; }

        public string DBHost { get; set; }
        public string DBPort { get; set; }
        public string DBUser { get; set; }
        public string DBPwd { get; set; }
        public string DBName { get; set; }
        public string DBTimeOut { get; set; }

    }
}
