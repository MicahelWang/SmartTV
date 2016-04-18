using System;
using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysCountyRepertory : BaseRepertory<CoreSysCounty, int>, ISysCountyRepertory
    {
        public override List<CoreSysCounty> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
