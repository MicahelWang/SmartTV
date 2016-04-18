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
    public class AuthForAppController : ApiController
    {
        private readonly IAuthCertigierManagerManager _authCertigier;

        public AuthForAppController(IAuthCertigierManagerManager authCertigier)
        {
            _authCertigier = authCertigier;
        }

        [HttpPost]
        public MsgResult Post(string deviceNo, string password)
        {
            var result = new MsgResult();
            try
            {
                var entity = _authCertigier.AuthorizationByDeviceNo(deviceNo, password);
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

        [HttpPost]
        public MsgResult AuthByUserName(string userName, string password)
        {
            var result = new MsgResult();
            try
            {
                var entity = _authCertigier.AuthorizationByUserName(userName, password);
                var auth = new AuthModel { Token = entity.Token, UserId = entity.UserId, UserName = entity.UserName };
                result.Data = auth;
            }
            catch (Exception ex)
            {

                result.HasError = true;
                result.Msg = ex.Message;
            }

            return result;
        }

        [AcceptVerbs("AuthByPhoneNo")]
        public MsgResult AuthByPhoneNo(string phoneNo, string password)
        {
            var result = new MsgResult();
            try
            {
                var entity = _authCertigier.AuthorizationByPhoneNo(phoneNo, password);
                var auth = new AuthModel { Token = entity.Token, UserId = entity.UserId, UserName = entity.UserName };
                result.Data = auth;
            }
            catch (Exception ex)
            {

                result.HasError = true;
                result.Msg = ex.Message;
            }

            return result;
        }
    }
}
