using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure.ManagerInterface
{
    public interface IScoreExchangManager : IBaseManager<ScoreExchang, ScoreExchangCriteria>
    {
        void AddOrUpdateScoreExchang(ScoreExchang scoreExchang);
    }
}
