using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;

namespace YeahCenter.Infrastructure
{
    public interface IRoleResourceRelationManager
    {

        List<ErpPowerRoleResourceRelation> GetAll();

        ErpPowerRoleResourceRelation GetEntity(string id);

        List<ErpPowerRole> Search();
    }
}