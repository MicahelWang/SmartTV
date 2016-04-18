using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using EntityFramework.Extensions;

using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class HCSJobRepertory : BaseRepertory<HCSDownLoadJob, string>, IHCSJobRepertory
    {
        public override List<HCSDownLoadJob> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HCSTaskCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.ServerId))
                query = query.Where(q => q.Id.Equals(criteria.JobId));


            return query.ToList();
        }
    }
}
