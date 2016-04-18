using System;
namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class TVChannelCriteria : BaseSearchCriteria
    {
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string Icon { get; set; }
        public string Category { get; set; }
        public string CategoryEn { get; set; }
        public string DefaultCode { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
