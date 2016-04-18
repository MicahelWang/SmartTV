using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class AuthTVTokenRepertory : BaseRepertory<AuthTVToken, string>, IAuthTVTokenRepertory
    {
        public override List<AuthTVToken> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as AuthTVTokenCriteria;
            var query = base.Entities.AsQueryable();
            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));
            if (!string.IsNullOrEmpty(criteria.AuthTicket))
                query = query.Where(q => q.AuthTicket.Equals(criteria.AuthTicket));
            if (!string.IsNullOrEmpty(criteria.AuthToken))
                query = query.Where(q => q.AuthToken.Equals(criteria.AuthToken));
            if (!string.IsNullOrEmpty(criteria.Code))
                query = query.Where(q => q.Code.Equals(criteria.Code));
            if (criteria.Type != null)
                query = query.Where(q => q.Type.Equals(criteria.Type.Value));
            return query.ToList();
        }


        public bool IsExtisCertigier(string authTicket, int type)
        {
            return Entities.AsQueryable().Any(m => m.AuthTicket == authTicket && m.ExpireTime > DateTime.Now && m.Type == type);
        }

        public AuthCertigierManager GetCertigier(string authTicket, int type)
        {
            throw new NotImplementedException();
        }


        public AuthTVToken CheckToken(RequestTokenParameter tokenParameter)
        {
            return Entities.AsQueryable().SingleOrDefault(
                m => m.AuthTicket == tokenParameter.Ticket &&
                    m.AuthToken == tokenParameter.Token &&
                    m.ExpireTime > DateTime.Now &&
                    m.Code == tokenParameter.Code);
        }
    }
}