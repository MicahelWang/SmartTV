using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;

namespace YeahTVApiLibrary.Manager
{
    public class GlobalConfigManager : BaseManager<GlobalConfig, GlobalConfigCriteria>, IGlobalConfigManager
    {
        private readonly IGlobalConfigRepertory globalConfigRepertory;
        private readonly IConstantSystemConfigManager constantSystemConfigManager;
        private readonly IRequestApiService requestApiService;

        public GlobalConfigManager(IGlobalConfigRepertory globalConfigRepertory,
            IRequestApiService requestApiService,
            IConstantSystemConfigManager constantSystemConfigManager)
            : base(globalConfigRepertory)
        {
            this.globalConfigRepertory = globalConfigRepertory;
            this.requestApiService = requestApiService;
            this.constantSystemConfigManager = constantSystemConfigManager;
        }

        public List<GlobalConfig> SearchAllType(GlobalConfigSearchInfo globalConfigSearchInfo)
        {
            var result = new List<GlobalConfig>();
            switch (globalConfigSearchInfo.GlobalConfigType)
            {
                case GlobalConfigType.Hotel:
                    var url = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + globalConfigSearchInfo.TypeId;
                    var hotels = JsonConvert.DeserializeObject<HotelEntity>(requestApiService.HttpRequest(url, "GET"));
                    result = globalConfigRepertory.SearchByHotelId(globalConfigSearchInfo.TypeId, hotels.BrandId, hotels.GroupId);
                    break;
                case GlobalConfigType.Brand:
                    var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetBrandUrl + globalConfigSearchInfo.TypeId;
                    var brand = JsonConvert.DeserializeObject<CoreSysBrand>(requestApiService.Get(requestHotelUrl));
                    result = globalConfigRepertory.SearchByBrandId(globalConfigSearchInfo.TypeId, brand.GroupId);
                    break;
                case GlobalConfigType.Group:
                    result = globalConfigRepertory.SearchByGroupId(globalConfigSearchInfo.TypeId);
                    break;
            }
            return result;
        }

        public string SearchKey(GlobalConfigSearchInfo searchInfo)
        {
            return SearchAllType(searchInfo).First(g => g.ConfigName.ToLower() == searchInfo.ConfigName.ToLower()).ConfigValue;
        }

        #region 配置项


        public string GetHotelScoreRate(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScoreRate");
        }
        public string GetHotelScoreGateWayUrl(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScoreGateWayUrl");
        }
        public string GetHotelScoreGateWayFrontSignKey(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScoreGateWayFrontSignKey");
        }
        public string GetHotelScoreGateWayBackSignKey(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScoreGateWayBackSignKey");
        }
        public string GetHotelScoreBusinessCode(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScoreBusinessCode");
        }
        public string GetHotelScoreGetTokenUrl(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScoreGetTokenUrl");
        }

        public string GetHotelScorePartnerId(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScorePartnerId");
        }
        private string GetHotelConfig(string hotelId, string configName)
        {
            string result;
            try
            {
                result =
                    SearchKey(new GlobalConfigSearchInfo()
                    {
                        ConfigName = configName,
                        TypeId = hotelId,
                        GlobalConfigType = GlobalConfigType.Hotel
                    });
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("读取配置项错误：项:{0} 酒店:{1} ", configName, hotelId));
            }
            return result;
        }


        #endregion
        public GlobalConfig GetGlobalConfig(GlobalConfig globalConfig)
        {
            return globalConfigRepertory.GetGlobalConfig(globalConfig);
        }
        public void AddGlobalConfig(GlobalConfig globalConfig)
        {

            globalConfigRepertory.Insert(globalConfig);
        }
        public void UpdateGlobalConfig(GlobalConfig globalConfig)
        {
            globalConfigRepertory.Update(globalConfig);
        }
        public void DeleteGlobalConfigById(string id)
        {
            globalConfigRepertory.Delete(t => t.Id == id);
        }

        public string GetHotelScoreChannelsource(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScoreChannelsource");
        }


        public string GetHotelIncreaseRatio(string hotelId)
        {
            return GetHotelConfig(hotelId, "ScoreIncreaseRatio");
        }
    }
}
