using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class PowerRoleResourceRelationRepertory : BaseRepertory<ErpPowerRoleResourceRelation, string>, IPowerRoleResourceRelationRepertory
    {
        public override List<ErpPowerRoleResourceRelation> Search(BaseSearchCriteria searchCriteria)
        {
            throw new System.NotImplementedException();
        }
    }
}