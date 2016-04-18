namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using System.Collections.Generic;

    public interface ISystemConfigManager
    {
        List<SystemConfig> Search(SystemConfigCriteria systemConfigCriteria);

        void AddSystemConfig(SystemConfig systemConfig);

        void UpdateSystemConfig(SystemConfig systemConfig);

        SystemConfig FindByKey(int configId);

        Dictionary<string, string> GetAllSysType();
    }
}
