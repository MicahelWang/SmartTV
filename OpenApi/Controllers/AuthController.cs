using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace OpenApi.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IAuthCertigierManagerManager _authCertigier;

        public AuthController(IAuthCertigierManagerManager authCertigier)
        {
            _authCertigier = authCertigier;
        }

        [HttpPost]
        public MsgResult Post(string username, string password)
        {
            var result = new MsgResult();
            try
            {
                var entity = _authCertigier.AuthenticationByUserName(username, password);
                var auth = new AuthModel { Token = entity.Token, UserId = entity.UserId,UserName = entity.UserName};
                result.Data = auth;
            }
            catch (Exception ex)
            {
                result.HasError=true;
                result.Msg = ex.Message;
            }
           
            return result;
        } 
        
    }
}
