using System;
using System.Collections.Generic;
using System.Linq;
using Library.Framework.Core.Time;

namespace Library.Framework.Core.Model
{
    [Serializable]
    public class DResult
    {
        private DateTime _timestamp;

        public bool Status
        {
            get
            {
                return this.Code == 0;
            }
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp
        {
            get
            {
                if (!(this._timestamp == DateTime.MinValue))
                    return this._timestamp;
                return Clock.Now;
            }
            set
            {
                this._timestamp = value;
            }
        }

        public DResult()
            : this(string.Empty, 0)
        {
        }

        public DResult(string message, int code = 0)
        {
            this.Message = message;
            this.Code = code;
        }

        public static DResult Success
        {
            get
            {
                return new DResult(string.Empty, 0);
            }
        }

        public static DResult Error(string message, int code = -1)
        {
            return new DResult(message, code);
        }

        public static DResult<T> Succ<T>(T data)
        {
            return new DResult<T>(data);
        }

        public static DResult<T> Error<T>(string message, int code = -1)
        {
            return new DResult<T>(message, code);
        }

        public static DResults<T> Succ<T>(IEnumerable<T> data, int count = -1)
        {
            if (count >= 0)
                return new DResults<T>(data, count);
            return new DResults<T>(data);
        }

        public static DResults<T> Errors<T>(string message, int code = -1)
        {
            return new DResults<T>(message, code);
        }
    }

    [Serializable]
    public class DResult<T> : DResult
    {
        public T Data { get; set; }

        public DResult()
            : this(default(T))
        {
        }

        public DResult(T data)
            : base(string.Empty, 0)
        {
            this.Data = data;
        }

        public DResult(string message, int code = -1)
            : base(message, code)
        {
        }
    }

    [Serializable]
    public class DResults<T> : DResult
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }

        public DResults()
            : this(string.Empty, -1)
        {
        }

        public DResults(string message, int code = -1)
            : base(message, code)
        {
        }

        public DResults(IEnumerable<T> list)
            : base(string.Empty, 0)
        {
            T[] objArray = list as T[] ?? list.ToArray<T>();
            this.Data = (IEnumerable<T>)objArray;
            this.Total = objArray.Length;
        }

        public DResults(IEnumerable<T> list, int total)
            : base(string.Empty, 0)
        {
            this.Data = list;
            this.Total = total;
        }

        public DResults(IPagedList<T> list)
            : base(string.Empty, 0)
        {
            this.Data = (IEnumerable<T>)((IList<T>)(list as T[]) ?? list.ToArray());
            this.Total = list.Total;
        }
    }
}
