using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApiLibrary.Manager
{
    public class TVHotelConfigManager :BaseManager<TVHotelConfig,HotelConfigCriteria>, ITVHotelConfigManager
    {

        private ITVHotelConfigRepertory tVHotelConfigRepertory;

        private readonly IRedisCacheService _redisCacheService;
        private readonly IConstantSystemConfigManager constantSystemConfigManager;

        public TVHotelConfigManager(ITVHotelConfigRepertory tVHotelConfigRepertory,
            IRedisCacheService redisCacheService
            , IConstantSystemConfigManager _constantSystemConfigManager)
            : base(tVHotelConfigRepertory)
        {
            this.tVHotelConfigRepertory = tVHotelConfigRepertory;
            _redisCacheService = redisCacheService;
            constantSystemConfigManager = _constantSystemConfigManager;
        }
     
        public TVHotelConfig GetHotelConfig(HotelConfigCriteria criteria)
        {
            return base.ModelRepertory.Search(criteria).FirstOrDefault();
        }
        [Cache]
        public List<TVHotelConfig> SearhTVHotelConfig(HotelConfigCriteria tVHotelConfigcriteria)
        {
            return base.ModelRepertory.Search(tVHotelConfigcriteria);
        }

        public void AddTVHotelConfig(TVHotelConfig tVHotelConfig)
        {
            //var list = GetAllFromCache().Where(m => m.HotelId == tVHotelConfig.HotelId && m.ConfigCode == tVHotelConfig.ConfigCode);
            var list = base.ModelRepertory.Search(new HotelConfigCriteria() { HotelId = tVHotelConfig.HotelId, ConfigCodes = tVHotelConfig.ConfigCode });

            if (!list.Any())
            {

                base.ModelRepertory.Insert(tVHotelConfig);
            }
            else
            {
                throw new Exception("该酒店配置已存在");
            }
        }

        public void UpdateTVHotelConfig(TVHotelConfig tVHotelConfig)
        {

            var tVHotelConfigDb = base.ModelRepertory.FindByKey(tVHotelConfig.Id);


            //tVHotelConfig.CopyTo(tVHotelConfigDb, new string[] { "Id" });

            tVHotelConfigDb.LastUpdateTime = tVHotelConfig.LastUpdateTime;
            tVHotelConfigDb.LastUpdater = tVHotelConfig.LastUpdater;
            tVHotelConfigDb.ConfigValue = tVHotelConfig.ConfigValue;
            tVHotelConfigDb.Active = tVHotelConfig.Active;

            base.ModelRepertory.Update(tVHotelConfigDb);
        }


        public TVHotelConfig GetEntity(int id)
        {
            //return GetAllFromCache().FirstOrDefault(m => m.Id == id);
            return base.ModelRepertory.Search(new HotelConfigCriteria() { }).FirstOrDefault(m => m.Id == id);
        }

        public void AddTVHotelConfig(List<TVHotelConfig> tVHotelConfigs)
        {

            var exitConfigs = base.ModelRepertory.Search(new HotelConfigCriteria { HotelId = tVHotelConfigs.Select(h => h.HotelId).FirstOrDefault() });
            var configsToAdd = new List<TVHotelConfig>();
            var configsToEdit = new List<TVHotelConfig>();
            tVHotelConfigs.ForEach(t =>
            {
                if (!exitConfigs.Any(e => e.ConfigCode.Equals(t.ConfigCode)))
                {
                    configsToAdd.Add(t);
                }
                else
                {
                    configsToEdit.Add(t);
                }
            });
            foreach (var item in configsToEdit)
            {
                TVHotelConfig entity = base.ModelRepertory.Search(new HotelConfigCriteria { HotelId = item.HotelId, ConfigCodes = item.ConfigCode }).FirstOrDefault();
                entity.ConfigValue = item.ConfigValue;
                entity.LastUpdateTime = DateTime.Now;
                entity.LastUpdater = "amdmin";
                base.ModelRepertory.Update(entity);
            }
            if (configsToAdd.Any())
            {
                base.ModelRepertory.Insert(configsToAdd);
            }

        }
        public List<string> SearchOnlyHotelId(BaseSearchCriteria searchCriteria)
        {
            return tVHotelConfigRepertory.SearchOnlyHotelId(searchCriteria);

        }

        public void AddHotelPaymentConfig(string hotelId)
        {
            var configCode = "HotelPayment";
            var hotelConfig = base.ModelRepertory.Search(new HotelConfigCriteria() { })
                    .FirstOrDefault(m => m.HotelId.Equals(hotelId) && m.ConfigCode.ToLower().Equals("HotelPayment".ToLower()));
            if (hotelConfig == null)
            {
                var hotelPayment = JsonConvert.DeserializeObject<HotelPayment>(constantSystemConfigManager.HotelPayment);
                AddTVHotelConfig(new TVHotelConfig()
                {
                    Active = true,
                    ConfigCode = configCode,
                    HotelId = hotelId,
                    ConfigName = "VOD支付相关配置",
                    ConfigValue = JsonConvert.SerializeObject(hotelPayment),
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    LastUpdater = "System"
                });
            }
        } 
        public int Update(System.Linq.Expressions.Expression<Func<TVHotelConfig, bool>> Predicate, System.Linq.Expressions.Expression<Func<TVHotelConfig, TVHotelConfig>> Updater)
        {
          return    base.ModelRepertory.Update(Predicate, Updater);
        }

       
    }
}
