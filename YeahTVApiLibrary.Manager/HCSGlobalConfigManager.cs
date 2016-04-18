using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Common;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApi.DomainModel;
using YeahTVApi.Entity;

namespace YeahTVApiLibrary.Manager
{
    public class HCSGlobalConfigManager : BaseManager<HCSConfig, HCSConfigCriteria>, IHCSGlobalConfigManager
    {
        private IRequestApiService _requestApiService;
        private IHCSConfigRepertory _hcsConfigRepertory;
        private IConstantSystemConfigManager _constantSystemConfigManager;

        public HCSGlobalConfigManager(IHCSConfigRepertory hcsConfigRepertory
                                        , IRequestApiService requestApiService
                                        , IConstantSystemConfigManager constantSystemConfigManager)
            : base(hcsConfigRepertory)
        {
            _hcsConfigRepertory = hcsConfigRepertory;
            _requestApiService = requestApiService;

            _constantSystemConfigManager = constantSystemConfigManager;
        }

        [UnitOfWork]
        public HCSGlobalConfig GetServerGlobalConfig(string serverId, string sign, string oldTaskNo)
        {
            HCSConfigCriteria criteria = new HCSConfigCriteria();
            criteria.ServerId = serverId;

            var config = _hcsConfigRepertory.Search(criteria).Where(p => p.Type == "Global").OrderByDescending(p => p.UpdateTime).FirstOrDefault();
            if (config == null)
            {
                throw new ApiException(ApiErrorType.Default, "取不到任务配置项，请确认已经设置该服务器的下载配置。");
            }

            // 数据库实例和领域实例转换
            HCSGlobalConfig result = JsonConvert.DeserializeObject<HCSGlobalConfig>(config.Value);
            result.ConfigNo = config.UpdateTime.ToString("yyyyMMddHHmmssfff");

            return result;
        }

        public void AddServerTaskConfig(string serverId)
        {
            HCSConfigCriteria criteria = new HCSConfigCriteria { ServerId = serverId, Type = "Task" };
            var configs = _hcsConfigRepertory.Search(criteria);
            var newConfig = new List<HCSConfig>();

            if (configs.FirstOrDefault(m => m.Type == "Task") == null)
            {
                var taskConfig = new HCSConfig()
                {
                    CreateTime = DateTime.Now,
                    Id = Guid.NewGuid().ToString("N"),
                    ServerId = serverId,
                    Type = "Task",
                    UpdateTime = DateTime.Now,
                    UpdateUser = "System",
                    Value = _constantSystemConfigManager.HCSTaskDefaultConfig
                };
                newConfig.Add(taskConfig);
            }

            if (configs.FirstOrDefault(m => m.Type == "Global") == null)
            {
                var globalConfig = new HCSConfig()
                    {
                        CreateTime = DateTime.Now,
                        Id = Guid.NewGuid().ToString("N"),
                        ServerId = serverId,
                        Type = "Global",
                        UpdateTime = DateTime.Now,
                        UpdateUser = "System",
                        Value = _constantSystemConfigManager.HCSGlobalDefaultConfig
                    };
                newConfig.Add(globalConfig);
            }

            if (newConfig.Count > 0)
                _hcsConfigRepertory.Insert(newConfig);


        }
    }
}
