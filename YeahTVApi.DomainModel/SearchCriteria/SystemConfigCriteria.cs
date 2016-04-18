namespace YeahTVApi.DomainModel.SearchCriteria
{
    public class SystemConfigCriteria : BaseSearchCriteria
    {
        public int? Id { get; set; }

        public string ConfigName { get; set; }

        public string ConfigValue { get; set; }

        public string AppType { get; set; }

        public bool? Enable { get; set; }
    }
}
