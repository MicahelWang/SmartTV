using System;
using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysRoleRepertory : BaseRepertory<ErpPowerRole, string>, ISysRoleRepertory
    {
        public override List<ErpPowerRole> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();

        }
    }
}