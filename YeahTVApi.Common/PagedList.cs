using System;
using System.Collections.Generic;
using System.Linq;

namespace YeahTVApi.Common
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList
    {
        int PageIndex { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
    }

    public class PageCondition
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortFiled { get; set; }
        public bool OrderAsc { get; set; }
    }

    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> : IList<T>, IPagedList
    {

    }

    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        private PagedList()
        {

        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize == 0)
            {
                this.TotalCount = source.Count();
                this.TotalPages = 1;
                this.AddRange(source.ToList());
                return;
            }

            int total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            if (total == 0)
            {
                return;
            }

            var query = source.Skip(pageIndex * pageSize).Take(pageSize);
            this.AddRange(query.ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            if (pageSize == 0)
            {
                pageSize = int.MaxValue;
            }
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        public PagedList(IList<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source);
        }
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            if (pageSize == 0)
            {
                pageSize = int.MaxValue;
            }
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source);
        }



        //public static PagedList<T2> Mapper<T,T2>(PagedList<T> source, IEnumerable<object> dest)
        //{
        //    var p = new PagedList<T>(dest, source);
        //}
        public PagedList<T2> MapperTo<T2>(Func<T, T2> selector)
        {
            var p = new PagedList<T2>
            {
                TotalCount = this.TotalCount,
                TotalPages = this.TotalPages,
                PageSize = this.PageSize,
                PageIndex = this.PageIndex
            };


            p.AddRange(this.Select(selector));
            return p;
        }


        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }


    }

    public class PagedViewList<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public List<T> Source { get; set; }
    }
}
