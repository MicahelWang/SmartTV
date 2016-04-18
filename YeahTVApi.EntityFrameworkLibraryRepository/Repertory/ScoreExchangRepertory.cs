using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class ScoreExchangRepertory : BaseRepertory<ScoreExchang, string>, IScoreExchangRepertory
    {
        public override List<ScoreExchang> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as ScoreExchangCriteria;
            var query = base.Entities.AsQueryable();

            if (criteria != null)
            {
                if (!string.IsNullOrWhiteSpace(criteria.OrderId))
                {
                    query = query.Where(s => s.OrderId.Equals(criteria.OrderId));
                }
            }

            return query.ToPageList(criteria);

        }
    }
}
