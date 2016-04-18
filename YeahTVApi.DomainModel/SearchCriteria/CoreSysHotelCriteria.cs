namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class CoreSysHotelCriteria : BaseSearchCriteria
    {
        public string HotelName { get; set; }
        public string GroupId { get; set; }
        public string BrandId { get; set; }
        public string TemplateId { get; set; }
    }
}
