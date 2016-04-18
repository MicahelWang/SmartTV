using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class DistributeModel
    {
        public string MovieId { get; set; }
        public string HotelNames { get; set; }
        public DistributeType DistributeType { get; set; }
        public string HotelIds { get; set; }

        public string HotelListJsonString { get; set; }
        public int PageIndex { get; set; }
    }
}