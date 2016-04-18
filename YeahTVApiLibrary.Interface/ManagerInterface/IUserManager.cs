using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ConditionModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IUserManager
    {

        List<ErpSysUser> GetAll();

        ErpSysUser GetEntity(string id);
        CoreSysLogin GetLoginAccount(string id);

        List<ErpSysUser> Search();


        bool Add(ErpSysUser entity);

        bool AddLoginAccount(CoreSysLogin login);

        bool Update(ErpSysUser entity);

        bool UpdateLoginAccount(CoreSysLogin login);

        bool Delete(string id);

        IPagedList<ErpSysUser> PagedList(int pageIndex, int pageSize, UserCondition condition);

        CurrentUser GetCurrentUserData(string userName);

        CoreSysLogin Login(LoginModel model);

        CoreSysLogin Login(string userId, string password);

        CoreSysLogin GetLoginByUserName(string username);

        void BatchDelete(string[] roles);
        List<ErpSysUser> SearchErpSysUser(ErpSysUserCriteria erpSysUserCriteria);
   
    }
}
