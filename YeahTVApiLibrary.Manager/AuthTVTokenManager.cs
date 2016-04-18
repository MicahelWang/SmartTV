using System;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.Models.PointsModels;
using YeahTVApiLibrary.Infrastructure;
using System.Linq;
using System.Text;
using YeahTVApi.Entity;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.DomainModel.SearchCriteria;
namespace YeahTVApiLibrary.Manager
{
    public class AuthTVTokenManager : IAuthTVTokenManager
    {
        private readonly IAuthTVTokenRepertory _autthTVToken;
        private readonly IConstantSystemConfigManager constantSystemConfigManager;
        private readonly ILogManager logManager;

        public AuthTVTokenManager(IAuthTVTokenRepertory authCertigier, ILogManager _logManager, IConstantSystemConfigManager _constantSystemConfigManager)
        {
            _autthTVToken = authCertigier;
            logManager = _logManager;
            constantSystemConfigManager = _constantSystemConfigManager;
        }

        public ResponseApiData<string> CreateToken(string authTicket, string code, int type, string sign, string signTime, int expiredMinus = 60)
        {
            ResponseApiData<string> ret = new ResponseApiData<string>() { Data = "", Message = "" };
            ret.Code = ApiErrorType.Parameter.ToInt();
            try
            {
                if (!CheckSignData(authTicket + signTime, sign))
                {
                    ret.Code = ApiErrorType.SignError.ToInt();
                    ret.Message = "Sign Error!";
                }
                else if (string.IsNullOrEmpty(authTicket))
                {
                    ret.Message = "票据不能为空!";
                }
                else if (string.IsNullOrEmpty(code))
                {
                    ret.Message = "标识不能为空!";
                }

                if (string.IsNullOrEmpty(ret.Message))
                {
                    var isHave = _autthTVToken.Search(new AuthTVTokenCriteria { AuthTicket = authTicket, Code = code, Type = type });
                    if (isHave.Count > 0)
                    {
                        var token = isHave.FirstOrDefault(m => m.ExpireTime > DateTime.Now);
                        if (token != null)
                        {
                            ret.Code = ApiErrorType.Success.ToInt();
                            ret.Data = token.AuthToken;
                        }
                        else
                        {
                            ret.Code = ApiErrorType.TicketExpired.ToInt();
                            ret.Message = "票据过期!";
                        }
                    }
                    else
                    {
                        ret.Code = ApiErrorType.Success.ToInt();
                        var entity = new AuthTVToken
                        {
                            Id = Guid.NewGuid().ToString("N").ToUpper(),
                            AuthToken = Guid.NewGuid().ToString("N").ToUpper(),
                            AuthTicket = authTicket,
                            CreateTime = DateTime.Now,
                            ExpireTime = DateTime.Now.AddMinutes(expiredMinus),
                            Code = code,
                            Type = type
                        };
                        _autthTVToken.Insert(entity);
                        ret.Data = entity.AuthToken;
                    }
                }
            }
            catch (Exception err)
            {
                ret.Code = ApiErrorType.System.ToInt();
                ret.Message = err.Message;
                logManager.SaveError("OpenApi 颁发Token异常", err, AppType.TV);
            }
            return ret;
        }

        private bool CheckSignData(string data, string checkSign)
        {
            string key = new StringBuilder().Append(data).Append(GetSignKey()).ToString().StringToMd5();
            if (string.IsNullOrWhiteSpace(checkSign) || !key.ToLower().Equals(checkSign.ToLower()))
            {
                return false;
            }
            return true;
        }
        private string GetSignData(string data)
        {
            return new StringBuilder().Append(data).Append(GetSignKey()).ToString().StringToMd5();
        }
        private string GetSignKey()
        {
            return constantSystemConfigManager.OpenAPIAuthSignPrivateKey;
        }

        public bool CheckTokenIsEffective(RequestTokenParameter tokenParameter)
        {
            return (tokenParameter != null && _autthTVToken.CheckToken(tokenParameter) != null);
        }
    }
}
