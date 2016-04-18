using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Mapping;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApi.Infrastructure;
using YeahTVApi.Manager;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Filter;

namespace YeahTVApi.Controllers
{
    public class TVHotelConfigController : BaseController
    {
        private ITVHotelConfigManager tVHotelConfigManager;
        private ILogManager logManager;

        public TVHotelConfigController(
            ITVHotelConfigManager tVHotelConfigManager,
            ILogManager logManager,
            IHttpContextService httpContextService)
        {
            this.tVHotelConfigManager = tVHotelConfigManager;
            this.logManager = logManager;
        }

        // GET: TvHotelConfig
        [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiObjectResult<Dictionary<string, string>> GetTVHotelUsingForDrop()
        {
            var list = GetConfigList();

            return new ApiObjectResult<Dictionary<string, string>> { obj = list };
        }

        [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiListResult<TVHotelConfig> GetTVHotelUsingForDropByHotelId()
        {
            //var hotelConfigs = tVHotelConfigManager.GetAllFromCache().Where(m => m.HotelId.Equals(Header.HotelID)).ToList();
            var hotelConfigs = tVHotelConfigManager.SearchFromCache(new HotelConfigCriteria() {HotelId = Header.HotelID });
            var list = GetConfigList();
            ApiListResult<TVHotelConfig> lis = new ApiListResult<TVHotelConfig>();
            lis.list = hotelConfigs.Where(h => list.Select(l => l.Key).Contains(h.ConfigCode)).ToList();
            return lis;
        }

        [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiResult AddTVHotelConfig(string strTVHotelConfigs, string hotelId)
        {

            List<TVHotelConfig> listBackupdevice = strTVHotelConfigs.JsonStringToObj<List<TVHotelConfig>>();
            foreach (TVHotelConfig item in listBackupdevice)
            {
                item.HotelId = hotelId;
                item.ConfigName = item.ConfigCode.ParseAsEnum<TvHotelConfigType>().GetDescription();
                item.CreateTime = DateTime.Now;
                item.LastUpdateTime = DateTime.Now;
                item.LastUpdater = Header.Guest;
            }
            var res = new ApiResult();
            try
            {
                tVHotelConfigManager.AddTVHotelConfig(listBackupdevice);
                return res.WithOk();
            }
            catch (Exception ex)
            {
                logManager.SaveError("添加失败", ex, AppType.CommonFramework);
                return res.WithError(ex.ToString());
            }
        }

        //edit
        [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiResult EditTVHotelConfig(List<TVHotelConfig> tVHotelConfig)
        {
            var res = new ApiResult();
            try
            {
                foreach (var item in tVHotelConfig)
                {
                    TVHotelConfig newtVHotelConfig = new TVHotelConfig() { Id = item.Id, ConfigValue = item.ConfigValue, Active = item.Active };
                    newtVHotelConfig.LastUpdateTime = DateTime.Now;
                    newtVHotelConfig.LastUpdater = "admin";
                    tVHotelConfigManager.UpdateTVHotelConfig(newtVHotelConfig);
                }
                return res.WithOk();

            }
            catch (Exception ex)
            {
                logManager.SaveError("修改失败", ex, AppType.CommonFramework);
                return res.WithError(ex.ToString());
            }
        }

        private static Dictionary<string, string> GetConfigList()
        {
            Dictionary<string, string> list = new Dictionary<string, string> 
            { 
               {TvHotelConfigType.ChannleAddress.ToString(),TvHotelConfigType.ChannleAddress.GetDescription().ToString()},
               {TvHotelConfigType.VodAddress.ToString(),TvHotelConfigType.VodAddress.GetDescription().ToString()},
               {TvHotelConfigType.PMSAddress.ToString(),TvHotelConfigType.PMSAddress.GetDescription().ToString()}
            };
            return list;
        }

        [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiListResult<TVHotelConfig> GetTVHotelConfigList(string hotelId)
        {
            var hotelConfigs = tVHotelConfigManager.SearchFromCache(new HotelConfigCriteria {HotelId = Header.HotelID });
            return new ApiListResult<TVHotelConfig>() { list = hotelConfigs };
        }

    }
}