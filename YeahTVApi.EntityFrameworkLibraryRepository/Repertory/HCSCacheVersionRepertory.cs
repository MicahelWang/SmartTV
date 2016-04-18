using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class HCSCacheVersionRepertory : BaseRepertory<HCSCacheVersion, string>, IHCSCacheVersionRepertory
    {
        public override List<HCSCacheVersion> Search(YeahTVApi.DomainModel.SearchCriteria.BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HCSCacheVersionCriteria;

            var query = base.Entities.AsQueryable();

            if (criteria != null)
            {
                if (!string.IsNullOrWhiteSpace(criteria.TypeId))
                {
                    query = query.Where(q => q.TypeId.Equals(criteria.TypeId));
                }
                if (!string.IsNullOrWhiteSpace(criteria.PermitionType))
                {
                    query = query.Where(q => q.PermitionType.Equals(criteria.PermitionType));
                }
            }
            
            return query.ToPageList(criteria);
        }
    }
}
