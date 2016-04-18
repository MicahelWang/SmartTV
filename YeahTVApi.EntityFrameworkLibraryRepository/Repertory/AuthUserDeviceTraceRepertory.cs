using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class AuthUserDeviceTraceRepertory : BaseRepertory<AuthUserDeviceTrace, string>, IAuthUserDeviceTraceRepertory
    {
        public override List<AuthUserDeviceTrace> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as AuthUserDeviceTraceCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.UserId))
                query = query.Where(q => q.UserId.Equals(criteria.UserId));

            if (!string.IsNullOrEmpty(criteria.DeviceNo))
                query = query.Where(q => q.DeviceNo.Equals(criteria.DeviceNo));


            return query.ToPageList(searchCriteria);

        }

        public AuthUserDeviceTrace GetDeviceTrace(string deviceNo)
        {
            return Entities.FirstOrDefault(m => m.DeviceNo == deviceNo);
        }
    }
}
