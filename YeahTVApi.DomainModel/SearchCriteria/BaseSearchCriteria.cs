namespace YeahTVApi.DomainModel.SearchCriteria
{
    public abstract class BaseSearchCriteria
    {
        public BaseSearchCriteria()
        {
            this.PageSize = Constant.PageSize;
            this.SortFiled = "Id";
        }

        public string Id { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalPages
        {
            get
            {
                var totalPage = 0;

                if (TotalCount > 0)
                {
                    totalPage = TotalCount % PageSize > 0 ? (TotalCount / PageSize) + 1 : TotalCount / PageSize;
                }

                return totalPage;
            }
        }

        public int TotalCount { get; set; }

        public string SortFiled { get; set; }

        public bool OrderAsc { get; set; }

        public bool NeedPaging { get; set; }
        public bool NeedNoTracking { get; set; }
    }
}
