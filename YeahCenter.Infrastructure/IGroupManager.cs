using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahCenter.Infrastructure
{
    public interface IGroupManager
    {
        CoreSysGroup GetGroup(string groupId);
        
        List<CoreSysGroup> GetAll();

        List<CoreSysGroup> Search(GroupCriteria searchCriteria);

        void Update(CoreSysGroup ef);

        void Insert(CoreSysGroup ef);
    }
}
