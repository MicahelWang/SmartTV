namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class HotelConfigCriteria : BaseSearchCriteria
    {
        public string HotelId { get; set; }

        public string ConfigCodes { get; set; }

        public bool? Active { get; set; }
    }
}
