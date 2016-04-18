using OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.PointsModels;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.Entity;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace OpenApi.Controllers
{
    [RoutePrefix("api/AuthTVToken")]
    public class AuthTVTokenController : ApiController
    {
        // GET: JinJiangIntegralExchange
        private IAuthTVTokenManager authTVTokenManager;

        public AuthTVTokenController(IAuthTVTokenManager _authTVTokenManager)
        {
            authTVTokenManager = _authTVTokenManager;
        }
        [HttpPost]
        [Route("GetToken")]
        public ResponseApiData<string> GetToken(RequestTokenData res)
        {
            string authTicket = res.authTicket, code = res.code;
            int type = res.type, expiredMinus = res.expiredMinus;
            string sign = res.sign, signTime = res.signTime;
            return authTVTokenManager.CreateToken(authTicket, code, type, sign, signTime, expiredMinus);
        }

        [HttpPost]
        [Route("TokenVerification")]
        public ResponseApiData<string> TokenVerification(RequestTokenParameter tokenParameter)
        {
            bool isEffective = authTVTokenManager.CheckTokenIsEffective(tokenParameter);
            if (isEffective)
                return new ResponseApiData<string> { Code = (int)ApiErrorType.Success, Message = "Token有效", Data = "" };
            else
                return new ResponseApiData<string> { Code = (int)ApiErrorType.TokenError, Message = "Token无效", Data = "" };
        }
    }

}