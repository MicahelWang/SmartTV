using System.Collections.Generic;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahCenter.Infrastructure
{
    public interface IRoleManager
    {

        List<ErpPowerRole> GetAll();

        ErpPowerRole GetEntity(string id);

        ErpPowerRole GetEntityName(string rolename);
        List<ErpPowerRole> Search();

        bool Add(ErpPowerRole entity);

        void BatchDelete(string[] roles);

        bool Update(ErpPowerRole entity);
        IPagedList<ErpPowerRole> PagedList(int pageIndex, int pageSize, string keyword);
        List<ErpPowerRoleResourceRelation> GetRelations(string id);
        IEnumerable<TreeNode> GetPower(string roleId);
        IEnumerable<ErpPowerResource> GetPowerResource(string roleId);

        int[] GetResourcesIdByRole(string roleId);

        bool UpdatePower(string id, int[] resouceIds);
    }

}