using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class AuthCertigierManagerRepertory : BaseRepertory<AuthCertigierManager, string>, IAuthCertigierManagerRepertory
    {
        public override List<AuthCertigierManager> Search(BaseSearchCriteria searchCriteria)
        {
            throw new NotImplementedException();
        }

        public bool IsExtisCertigier(string userId, int type)
        {
            return Entities.AsQueryable().Any(m => m.UserId == userId && m.ExpireTime > DateTime.Now && m.Type == type);
        }

        public AuthCertigierManager GetCertigier(string token, int type)
        {
            return
                Entities.AsQueryable().OrderByDescending(m => m.ExpireTime)
                    .FirstOrDefault(m => m.Token == token && m.ExpireTime > DateTime.Now && m.Type == type);
        }

        public AuthCertigierManager GetCertigierByUserId(string userId, int type)
        {
            return
              Entities.AsQueryable().OrderByDescending(m => m.ExpireTime)
                  .FirstOrDefault(m => m.UserId == userId && m.ExpireTime > DateTime.Now && m.Type == type);
        }
    }
}