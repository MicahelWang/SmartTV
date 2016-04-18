using System;
namespace YeahTVApi.DomainModel.Models
{
    public class CoreSysAttachment : BaseEntity<int>
    {

        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FilePath { get; set; }

        public decimal FileSize { get; set; }

        public string Unit { get; set; }

        public DateTime CrateTime { get; set; }
    }
}
