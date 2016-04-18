namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;
    using System.Collections.Generic;
    using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models.PointsModels;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;

    public interface IAuthTVTokenManager
    {
        /// <summary>
        /// 颁发Token
        /// </summary>
        /// <param name="authTicket">票据</param>
        /// <param name="code">标识</param>
        /// <param name="type">类型</param>
        /// <param name="sign">签名</param>
        /// <param name="signTime">签名时间戳</param>
        /// <returns>Token</returns>
        ResponseApiData<string> CreateToken(string authTicket, string code, int type, string sign, string signTime, int expiredMinus);

        bool CheckTokenIsEffective(RequestTokenParameter tokenParameter);
    }
}