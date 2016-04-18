using System;
using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysCityRepertory : BaseRepertory<CoreSysCity, int>, ISysCityRepertory
    {
        public override List<CoreSysCity> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
