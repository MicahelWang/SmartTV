using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EntityFramework.Extensions;

using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class HCSConfigRepertory : BaseRepertory<HCSConfig, string>, IHCSConfigRepertory
    {
        //public List<HCSConfig> GetAllWithInclude()
        //{
        //    return Entities.ToList();
        //}

        public override List<HCSConfig> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HCSConfigCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.ServerId))
                query = query.Where(q => q.ServerId.Equals(criteria.ServerId));

            if (!string.IsNullOrEmpty(criteria.Type))
                query = query.Where(q => q.Type.Equals(criteria.Type));

            return query.ToList();
        }
    }
}
