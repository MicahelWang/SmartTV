using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure.ManagerInterface
{
    public interface IGlobalConfigManager : IBaseManager<GlobalConfig, GlobalConfigCriteria>
    {
        [Cache]
        List<GlobalConfig> SearchAllType(GlobalConfigSearchInfo globalConfigSearchInfo);
        string SearchKey(GlobalConfigSearchInfo searchInfo);
        GlobalConfig GetGlobalConfig(GlobalConfig globalConfig);
        void UpdateGlobalConfig(GlobalConfig globalConfig);
        void AddGlobalConfig(GlobalConfig globalConfig);
        void DeleteGlobalConfigById(string Id);
        [Cache]
        string GetHotelScoreRate(string hotelId);
        [Cache]
        string GetHotelScoreGateWayUrl(string hotelId);
        [Cache]
        string GetHotelScoreGateWayFrontSignKey(string hotelId);
        [Cache]
        string GetHotelScoreGateWayBackSignKey(string hotelId);
        [Cache]
        string GetHotelScoreBusinessCode(string hotelId);
        [Cache]
        string GetHotelScorePartnerId(string hotelId);
        [Cache]
        string GetHotelScoreGetTokenUrl(string hotelId);
        [Cache]
        string GetHotelScoreChannelsource(string hotelId);
         [Cache]
        string GetHotelIncreaseRatio(string hotelId);
    }
}
