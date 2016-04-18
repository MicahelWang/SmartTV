using System.Collections.Concurrent;
using System.Collections.Generic;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCenter.Infrastructure
{
    public interface IBehaviorLogManager
    {
        BehaviorLog GetById(string id);
        List<BehaviorLog> Search(LogCriteria criteria);

        Dictionary<string, double> GetHotelModuleUsedTime(DashboardCriteria criteria);
        Dictionary<string, double> GetHotelMovieVodOfDay(DashboardCriteria criteria);
        Dictionary<string, double> GetHotelChannelUsedTime(DashboardCriteria criteria);
        void RefreshBehaviorLogDashBoard();
    }
}
