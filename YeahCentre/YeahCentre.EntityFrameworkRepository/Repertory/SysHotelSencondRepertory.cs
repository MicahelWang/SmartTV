using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysHotelSencondRepertory : BaseRepertory<CoreSysHotelSencond, string>, ISysHotelSencondRepertory
    {
        public override List<CoreSysHotelSencond> Search(BaseSearchCriteria searchCriteria)
        {

            return null;
        }
    }
}
