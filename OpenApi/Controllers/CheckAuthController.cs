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

    public class CheckAuthController : ApiController
    {
        private readonly IAuthCertigierManagerManager _authCertigier;

        public CheckAuthController(IAuthCertigierManagerManager authCertigier)
        {
            _authCertigier = authCertigier;
        }

        public MsgResult Get(string token,string deviceNo)
        {
            var result = new MsgResult();
            try
            {
                var entity = _authCertigier.AuthenticationByDeviceNo(deviceNo, token);
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

        public MsgResult GetByUserName(string userName,string token)
        {
            var result = new MsgResult();
            try
            {
                var entity = _authCertigier.AuthenticationByUserName(userName, token);
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

        [AcceptVerbs("GetByPhoneNo")]
        public MsgResult GetByPhoneNo(string token)
        {
            var result = new MsgResult();
            try
            {
                var entity = _authCertigier.AuthenticationByPhoneNo(token);
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
