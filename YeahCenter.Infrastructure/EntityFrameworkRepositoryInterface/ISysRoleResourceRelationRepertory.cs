using System.Collections.Generic;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCenter.Infrastructure
{
    public interface ISysRoleResourceRelationRepertory : IBsaeRepertory<ErpPowerRoleResourceRelation>
    {
        IEnumerable<ErpPowerRoleResourceRelation> GetByRole(string id);
    }
}