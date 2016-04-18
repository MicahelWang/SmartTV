using System;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable()]
    public class SysPowersView
    {
        public int FuncId { get; set; }
        public string Type { get; set; }
        public string Path { get; set; }
        public string DisplayName { get; set; }
        public int ParentFunId { get; set; }
        public int Orders { get; set; }
        public int Display { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Has { get; set; }
    }
}