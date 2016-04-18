using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public  class SysRoleResourceRelationRepertory:BaseRepertory<ErpPowerRoleResourceRelation,string>,ISysRoleResourceRelationRepertory
    {
        public override List<ErpPowerRoleResourceRelation> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ErpPowerRoleResourceRelation> GetByRole(string id)
        {
            var query =  Entities.Where(m => m.RoleId == id).ToList();
            return query;
        }
    }
}
