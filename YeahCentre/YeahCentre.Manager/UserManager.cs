using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ConditionModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentre.Manager
{
    public class UserManager : IUserManager
    {

        private readonly ISysUserRepertory _repertory;
        private readonly ISysLoginRepertory _loginRepertory;


        private readonly IRoleManager _roleManager;

        public UserManager(ISysUserRepertory repertory, ISysLoginRepertory loginRepertory, IRoleManager roleManager)
        {
            _repertory = repertory;
            _loginRepertory = loginRepertory;
            _roleManager = roleManager;
        }

        public List<ErpSysUser> GetAll()
        {
            return _repertory.GetAll();
        }

        public ErpSysUser GetEntity(string id)
        {
            return _repertory.FindByKey(id);
        }

        public CoreSysLogin GetLoginAccount(string id)
        {
            return _loginRepertory.FindByKey(id);
        }

        public List<ErpSysUser> Search()
        {
            return _repertory.GetAll();
        }

        public bool Add(ErpSysUser entity)
        {
            entity.Id = string.IsNullOrWhiteSpace(entity.Id) ? Guid.NewGuid().ToString("N").ToUpper() : entity.Id;
            _repertory.Insert(entity);
            return true;
        }

        public bool AddLoginAccount(CoreSysLogin login)
        {
            login.Password = login.Password.ToPassWordString();
            _loginRepertory.Insert(login);
            return true;
        }

        public bool Update(ErpSysUser entity)
        {
            var user = _repertory.FindByKey(entity.Id);
            user.UserName = entity.UserName;
            user.GroupId = entity.GroupId;
            user.HotelId = entity.HotelId;
            user.State = entity.State;
            user.Phone = entity.Phone;
            user.Email = entity.Email;
            user.RoleId = entity.RoleId;
            user.UserType = entity.UserType;
            user.ModifyDate = DateTime.Now;
            user.ModifyUser = entity.ModifyUser;
            _repertory.Update(user);
            return true;
        }

        public bool UpdateLoginAccount(CoreSysLogin login)
        {
            var entity = _loginRepertory.FindByKey(login.Id);
            entity.LoginName = login.LoginName;
            entity.UserName = login.LoginName;
            entity.State = login.State;
            _loginRepertory.Update(entity);
            return true;
        }

        public bool Delete(string id)
        {
            var user = _repertory.FindByKey(id);
            user.IsDelete = true;
            _repertory.Update(user);
            return true;
        }

        public IPagedList<ErpSysUser> PagedList(int pageIndex, int pageSize, UserCondition condition)
        {
            return _repertory.PagedList(pageIndex, pageSize, condition);
        }

        public CurrentUser GetCurrentUserData(string userName)
        {
            var currentUser = _repertory.GetCurrentUserData(userName);
            if (currentUser == null) return null;
            var funList = _roleManager.GetPowerResource(currentUser.RoleId);
            currentUser.FunList = funList.ToList();
            return currentUser;
        }

        public CoreSysLogin Login(LoginModel model)
        {
            var query = _loginRepertory.Login(model);
            return query;
        }

        public CoreSysLogin Login(string userId, string password)
        {
            var query = _loginRepertory.Login(userId, password);
            return query;
        }

        public CoreSysLogin GetLoginByUserName(string username)
        {
            var query = _loginRepertory.GetLoginByUserName(username);
            return query;
        }

        public void BatchDelete(string[] userIds)
        {
            _repertory.BatchDelete(userIds);
        }

        public List<ErpSysUser> SearchErpSysUser(ErpSysUserCriteria erpSysUserCriteria)
        {
            return _repertory.Search(erpSysUserCriteria);
        }
    }
}
