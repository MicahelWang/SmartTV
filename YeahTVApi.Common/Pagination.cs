namespace YeahTVApi.Common
{
    public class Pagination
    {
        public long TotleCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string CallBackUrl { get; set; }
        public string JqUpdateElement { get; set; }
    }
}