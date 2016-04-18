using YeahTVApi.DomainModel.Models.DataModel;

namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.SearchCriteria;
    using System;
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Common;

    public interface IMongoDeviceTraceManager 
    {
        List<HotelStartPercentage> GetHotelStartPercentage(DashboardCriteria criteria);
        void RefreshHotelStartPercentage();
    }
}
