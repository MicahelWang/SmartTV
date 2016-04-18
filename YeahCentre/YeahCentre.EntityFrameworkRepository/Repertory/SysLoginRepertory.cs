using System;
using System.Collections.Generic;
using System.Linq;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysLoginRepertory : BaseRepertory<CoreSysLogin, string>, ISysLoginRepertory
    {
        public override List<CoreSysLogin> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public CoreSysLogin Login(LoginModel model)
        {
            model.Password = model.Password.ToPassWordString();
           return Entities.FirstOrDefault(m => m.LoginName == model.UserName && m.Password == model.Password && !m.IsDelete);
        }

        public CoreSysLogin Login(string userId, string password)
        {
            password = password.ToPassWordString();
            return Entities.FirstOrDefault(m => m.Id == userId && m.Password == password && !m.IsDelete);
        }

        public CoreSysLogin GetLoginByUserName(string username)
        {
            return Entities.FirstOrDefault(m => m.LoginName== username && !m.IsDelete);
        }
    }
}
