using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface IDashBoardManager
    {
        List<HotelInfoStatistics> GetStatisticsHotelList(List<CoreSysHotel> hotelList);
         
    }
}
