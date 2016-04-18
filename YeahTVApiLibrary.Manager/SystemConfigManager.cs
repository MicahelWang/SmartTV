using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;

using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel;
namespace YeahTVApiLibrary.Manager
{
    public class SystemConfigManager : ISystemConfigManager
    {
        private IRedisCacheService _redisCacheService;
        private ISystemConfigRepertory systemRepertory;

        public SystemConfigManager(IRedisCacheService redisCacheService,
            ISystemConfigRepertory systemRepertory)
        {
            this._redisCacheService = redisCacheService;
            this.systemRepertory = systemRepertory;
        }

        #region Redis

        public List<SystemConfig> GetAllFromCache()
        {
            if (_redisCacheService.IsSet(Constant.SystemConfigKey))
                return _redisCacheService.Get<List<SystemConfig>>(Constant.SystemConfigKey);

            var configs = systemRepertory.GetAll();
            _redisCacheService.Set(Constant.SystemConfigKey, configs);
            return configs;
        }
        private void UpdateCache()
        {
            var configs = systemRepertory.GetAll();
            _redisCacheService.Set(Constant.SystemConfigKey, configs);
        }

        #endregion

        public List<SystemConfig> Search(SystemConfigCriteria systemConfigCriteria)
        {
            return systemRepertory.Search(systemConfigCriteria);
        }

        public void AddSystemConfig(SystemConfig systemConfig)
        {
            List<SystemConfig> listsystemConfig = systemRepertory.Search(new SystemConfigCriteria() { ConfigName = systemConfig.ConfigName });
            if (listsystemConfig == null || !listsystemConfig.Any())
            {
                systemRepertory.Insert(systemConfig);
                UpdateCache();
            }
            else
            {
                throw new Exception("该配置名称已存在");
            }

        }

        public void UpdateSystemConfig(SystemConfig systemConfig)
        {
            try
            {
                systemRepertory.Update(systemConfig);
                UpdateCache();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public SystemConfig FindByKey(int configId)
        {
            return this.GetAllFromCache().SingleOrDefault(m => m.Id == configId);
        }

        public Dictionary<string, string> GetAllSysType()
        {
            var dic = new Dictionary<string, string>();

            foreach (int item in Enum.GetValues(typeof(SystemConfigTypeEnum)))
            {
                var strName = Enum.GetName(typeof(SystemConfigTypeEnum), item);//获取名称
                var strVaule = item.ToString();//获取值
                dic.Add(strName, strName);
            }

            return dic;
        }
    }
}
