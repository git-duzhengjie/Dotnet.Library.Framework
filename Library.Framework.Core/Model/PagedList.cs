using System.Collections.Generic;
using System.Linq;

namespace Library.Framework.Core.Model
{
    public class PagedList<TEntity> : IPagedList<TEntity>
    {
        public IList<TEntity> DataList { get; set; }

        public PagedList()
        {
            this.Size = 1;
            this.DataList = (IList<TEntity>)new List<TEntity>();
        }

        public PagedList(IQueryable<TEntity> queryable, int index, int size)
          : this()
        {
            int num = queryable.Count<TEntity>();
            this.Total = num;
            this.Pages = num / size;
            if (num % size > 0)
                ++this.Pages;
            this.Size = size;
            this.Index = index;
            this.AddRange((IEnumerable<TEntity>)queryable.Skip<TEntity>((index - 1) * size).Take<TEntity>(size).ToList<TEntity>());
        }

        public IList<TEntity> ToArray()
        {
            if (this.DataList != null)
                return (IList<TEntity>)this.DataList.ToArray<TEntity>();
            return (IList<TEntity>)null;
        }

        public void AddRange(IEnumerable<TEntity> entityList)
        {
            if (entityList == null)
                return;
            if (this.DataList == null)
                this.DataList = (IList<TEntity>)new List<TEntity>();
            foreach (TEntity entity in entityList)
                this.DataList.Add(entity);
        }

        public PagedList(IList<TEntity> list, int index, int size)
          : this()
        {
            this.Total = list.Count<TEntity>();
            this.Pages = this.Total / size;
            if (this.Total % size > 0)
                ++this.Pages;
            this.Size = size;
            this.Index = index;
            this.AddRange((IEnumerable<TEntity>)list.Skip<TEntity>((index - 1) * size).Take<TEntity>(size).ToList<TEntity>());
        }

        public PagedList(IEnumerable<TEntity> enumerable, int index, int size, int total)
          : this()
        {
            this.Total = total;
            this.Pages = this.Total / size;
            if (this.Total % size > 0)
                ++this.Pages;
            this.Size = size;
            this.Index = index;
            this.AddRange(enumerable);
        }

        public PagedList<T> ConvertData<T>(IEnumerable<T> enumerable)
        {
            return new PagedList<T>(enumerable, this.Index, this.Size, this.Total);
        }

        public int Index { set; get; }

        public int Size { set; get; }

        public int Total { set; get; }

        public int Pages { set; get; }

        public bool HasPrev
        {
            get
            {
                return this.Index > 1;
            }
        }

        public bool HasNext
        {
            get
            {
                return this.Index < this.Pages;
            }
        }
    }
}
