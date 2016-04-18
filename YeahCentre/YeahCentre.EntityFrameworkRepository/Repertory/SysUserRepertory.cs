using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ConditionModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysUserRepertory : BaseRepertory<ErpSysUser, string>, ISysUserRepertory
    {
        public override List<ErpSysUser> Search(BaseSearchCriteria searchCriteria)
        {
               
            var criteria = searchCriteria as ErpSysUserCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.UserId))
                query = query.Where(q =>q.Id.Equals(criteria.UserId));

            if (!string.IsNullOrEmpty(criteria.UserName))
                query = query.Where(q => q.UserName.Contains(criteria.UserName));


            return query.ToPageList(searchCriteria);

        }

        public IPagedList<ErpSysUser> PagedList(int pageIndex, int pageSize, UserCondition condition)
        {
            var query = this.Entities.Include("CoreSysLogin").Where(m => !m.IsDelete);
            if (!string.IsNullOrEmpty(condition.UGroupId))
                query = query.Where(m => m.GroupId == condition.UGroupId);
            if (!string.IsNullOrEmpty(condition.UHotelId))
                query = query.Where(m => m.HotelId == condition.UHotelId);
            if (condition.UState != 0 && !condition.UState.IsNullOrEmpty())
                query = query.Where(m => m.State == condition.UState);
            if (condition.EUserType != 0 && !condition.EUserType.IsNullOrEmpty())
                query = query.Where(m => m.UserType == condition.EUserType);
            if (!string.IsNullOrEmpty(condition.KeyWord))
                query = query.Where(m => m.UserName.Contains(condition.KeyWord) ||m.CoreSysLogin.LoginName.Contains(condition.KeyWord)|| m.Phone.Contains(condition.KeyWord) || m.Email.Contains(condition.KeyWord));

            query = query.OrderBy(m => m.CreateDate);
            return new PagedList<ErpSysUser>(query, pageIndex, pageSize);

        }

        public CurrentUser GetCurrentUserData(string userName)
        {
            var intNormalVal = StateEnum.Normal.ToInt();
            var query = this.Entities.Include("CoreSysLogin").Where(m => !m.IsDelete
                && m.State == intNormalVal
                && m.CoreSysLogin.LoginName == userName).Select(m => new CurrentUser()
                {
                    UID = m.Id,
                    Account = m.CoreSysLogin.UserName,
                    ChineseName = m.UserName,
                    RoleId = m.RoleId,
                    UserType=m.UserType
                });
            return query.FirstOrDefault();
        }

        public void BatchDelete(string[] userIds)
        {
            var resultSet = this.Entities.Where(m => userIds.Contains(m.Id));
            foreach (var user in resultSet)
            {
                user.IsDelete = true;
                Update(user);
            }
        }
    }
}