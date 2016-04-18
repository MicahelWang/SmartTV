using System;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    [Serializable]
    public class TreeNode
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool ischecked { get; set; }
        public bool open { get; set; }
        public string icon { get; set; }
        public bool drag { get; set; }
    }
}
