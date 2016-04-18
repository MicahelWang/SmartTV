using System;

namespace YeahTVApi.DomainModel.Models
{
    public class VODRequest : BaseEntity<string>
    {
        public string MovieId { get; set; }
        public DateTime CreateTime { get; set; }
        public string SeriseCode { get; set; }
        public int? ResultType { get; set; }
        public string ResultMessage { get; set; }
        public string PayInfo { get; set; }


    }
}