namespace YeahTVApi.DomainModel.Models
{
    public partial class CoreSysProvince : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int ParentId { get; set; }
    }
}
