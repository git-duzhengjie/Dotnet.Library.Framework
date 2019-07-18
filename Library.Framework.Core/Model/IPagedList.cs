using System.Collections.Generic;

namespace Library.Framework.Core.Model
{
    public interface IPagedList<TEntity>
    {
        IList<TEntity> DataList { get; set; }

        int Index { get; }

        int Size { get; }

        int Total { get; }

        int Pages { get; }

        bool HasPrev { get; }

        bool HasNext { get; }

        PagedList<T> ConvertData<T>(IEnumerable<T> enumerable);

        IList<TEntity> ToArray();

        void AddRange(IEnumerable<TEntity> entityList);
    }
}
