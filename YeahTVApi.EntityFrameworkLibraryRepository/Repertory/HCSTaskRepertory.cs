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
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class HCSTaskRepertory : BaseRepertory<HCSDownloadTask, string>, IHCSTaskRepertory
    {
        //public List<HCSDownloadTask> GetAllWithInclude()
        //{
        //    return Entities.Include("HCSDownLoadJobs").ToList();
        //}

        public override List<HCSDownloadTask> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HCSTaskCriteria;

            var query = base.Entities.Include("HCSDownLoadJobs").AsQueryable();

            if (!string.IsNullOrEmpty(criteria.ServerId))
                query = query.Where(q => q.ServerId.Equals(criteria.ServerId));

            if (!string.IsNullOrEmpty(criteria.JobId))
                query = query.Where(q => q.HCSDownLoadJobs.Any(qq => qq.Id == criteria.JobId));

            if (!string.IsNullOrEmpty(criteria.Type))
                query = query.Where(q => q.Type.Equals(criteria.Type));

            if (criteria.NotSuccessResultStatus)
            {
                string resultStatus = DownloadStatus.Success.ConvertToString();
                query = query.Where(q => !q.ResultStatus.Equals(resultStatus));
            }

            if (!string.IsNullOrEmpty(criteria.TaskNo))
                query = query.Where(q => q.TaskNo.Equals(criteria.TaskNo));

            return query.ToList();
        }
        public int GetRecordCount()
        {
            return Entities.Include("HCSDownLoadJobs").Count();
        }
    }
}
