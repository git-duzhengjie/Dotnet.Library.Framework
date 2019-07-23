using Library.Framework.Core.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Framework.Core.Utility
{
    public static class MongodbClient<T> where T : class
    {
        #region MongodbInfoClient 获取mongodb
        /// <summary>
        /// 获取mongodb实例
        /// </summary>  
        public static IMongoCollection<T> MongodbInfoClient(MongodbHost host)
        {
            try
            {
                var hosts = host.DBHost.Split(new char[] { '|' });
                var servers = new List<MongoServerAddress>();

                foreach (var h in hosts)
                {
                    servers.Add(new MongoServerAddress(h, int.Parse(host.DBPort)));
                }
                var mc = new MongoClient(new MongoClientSettings
                {
                    Servers = servers,
                    Credentials = new List<MongoCredential> { MongoCredential.CreateCredential("admin", host.DBUser, host.DBPwd) },
                    SocketTimeout = TimeSpan.FromSeconds(double.Parse(host.DBTimeOut))
                });

                var Database = mc.GetDatabase(host.DBName);
                return Database.GetCollection<T>(host.Table);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion
    }

}
