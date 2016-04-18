using System;
using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysProvinceRepertory : BaseRepertory<CoreSysProvince, int>, ISysProvinceRepertory
    {
        public override List<CoreSysProvince> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
