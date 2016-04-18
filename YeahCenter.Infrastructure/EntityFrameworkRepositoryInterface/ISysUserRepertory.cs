using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ConditionModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCenter.Infrastructure
{
    public interface ISysUserRepertory : IBsaeRepertory<ErpSysUser>
    {
        IPagedList<ErpSysUser> PagedList(int pageIndex, int pageSize, UserCondition condition);

        CurrentUser GetCurrentUserData(string userName);
        void BatchDelete(string[] userIds);
    }

}
