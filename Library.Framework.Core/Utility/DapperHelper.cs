using Dapper;
using Library.Framework.Core.Enum;
using Library.Framework.Core.Model;
using MySql.Data.MySqlClient;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.Framework.Core.Utility
{
    public class DapperHelper
    {
        private static Dictionary<string, DapperHelper> _connPool=new Dictionary<string, DapperHelper>();

        private IDbConnection _dbConnection = null;

        /// 定义一个标识确保线程同步        
        private static readonly object locker = new object();

        public bool IsClosed() {
            return _dbConnection.State==ConnectionState.Closed;
        }

        public delegate void Process();

        public DapperHelper(IDbConnection connection) {
            _dbConnection = connection;
        }

        private IDbTransaction _transaction;
        private bool _isBuffer=false;
        private int? _timeOut = null;
        

        /// <summary>
        /// 获取实例，这里为单例模式，保证只存在一个实例
        /// </summary>
        /// <returns></returns>
        public static DapperHelper GetInstance(DatabaseConfiguration config)
        {
            lock (locker)
            {
                var conn = config.Connection;
                if (_connPool != null && _connPool.ContainsKey(conn))
                {
                    if (_connPool[conn].IsClosed())
                        _connPool[conn].Open();
                    return _connPool[conn];
                }
                else
                {
                    IDbConnection dbConnection=null;
                    switch (config.DBtype)
                    {
                        case DBType.SqlServer:
                            dbConnection = new SqlConnection(config.Connection);
                            break;
                        case DBType.PgSql:
                            dbConnection = new NpgsqlConnection(config.Connection);
                            break;
                        case DBType.MySql:
                            dbConnection = new MySqlConnection(config.Connection);
                            break;
                    }
                    if (dbConnection != null)
                    {
                        var dapper = new DapperHelper(dbConnection);
                        _connPool.Add(config.Connection, dapper);
                        return dapper;
                    }
                    else
                        throw new Exception("不存在的DBTYPE！");
                }
            }
        }

        public void SetBuffer(bool isBuffer) {
            _isBuffer = isBuffer;
        }

        public void SetTimeOut(int timeOut) {
            _timeOut = timeOut;
        }

        private void Open()
        {
            _dbConnection.Open();
        }

        public T QueryFirstOrDefault<T>(string sql, object param = null)
        {
            return _dbConnection.QueryFirstOrDefault<T>(sql, param,_transaction);
        }


        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            return _dbConnection.QueryFirstOrDefaultAsync<T>(sql, param,_transaction);
        }

        public IEnumerable<T> Query<T>(string sql, object param = null)
        {
            return _dbConnection.Query<T>(sql, param, _transaction, _isBuffer, _timeOut, null);
        }

        public IPagedList<T> PagedQuery<T>(string sql, object param = null,int index=0,int size=0)
        {
            if (index == 0 && size == 0)
                return new PagedList<T> { DataList = _dbConnection.Query<T>(sql, param, _transaction, _isBuffer, _timeOut, null).ToList() };
            else {
                int offset = (index - 1) * size;
                var tmp=Regex.Split(sql, "\\s+").ToList();
                var end = tmp.IndexOf("from");
                var sqlCount = $"select count(*) from {string.Join(" ",tmp[end+1])}";
                int total = QueryFirstOrDefault<int>(sqlCount,param);
                sql += $" limit {size} offset {offset}";
                var data = Query<T>(sql,param);
                return new PagedList<T> {
                    Index=index,
                    Size=size,
                    Pages=total/size,
                    Total=total,
                    DataList=data.ToList()
                };
            }
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null)
        {
            return _dbConnection.QueryAsync<T>(sql, param, _transaction, _timeOut, null);
        }

        public int Execute(string sql, object param = null)
        {
            return _dbConnection.Execute(sql, param, _transaction, _timeOut, null);
        }

        public Task<int> ExecuteAsync(string sql, object param = null)
        {
            return _dbConnection.ExecuteAsync(sql, param, _transaction, _timeOut, null);
        }

        public T ExecuteScalar<T>(string sql, object param = null)
        {
            return _dbConnection.ExecuteScalar<T>(sql, param, _transaction, _timeOut, null);
        }


        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null)
        {
            return _dbConnection.ExecuteScalarAsync<T>(sql, param, _transaction, _timeOut, null);
        }

        public SqlMapper.GridReader QueryMultiple(string sql, object param = null)
        {
            return _dbConnection.QueryMultiple(sql, param, _transaction, _timeOut, null);
        }

        public Task<SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null)
        {
            return _dbConnection.QueryMultipleAsync(sql, param, _transaction, _timeOut, null);
        }

        public void ExcuteTransaction(Process t ) {
            try
            {
                _transaction = _dbConnection.BeginTransaction();
                t();
                _transaction.Commit();
            }
            catch (Exception ex) {
                _transaction.Rollback();
                throw ex;
            }
        }

    }
     
}
