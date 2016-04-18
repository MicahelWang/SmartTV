using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure.ManagerInterface
{
    public interface IHCSGlobalConfigManager : IBaseManager<HCSConfig, HCSConfigCriteria>
    {
        [Cache]
        HCSGlobalConfig GetServerGlobalConfig(string serverId, string sign, string oldTaskNo);

        void AddServerTaskConfig(string serverId);
    }
}
