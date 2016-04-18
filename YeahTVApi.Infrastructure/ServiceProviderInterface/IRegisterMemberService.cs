namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity;
    using System;

    public interface IRegisterMemberService : ICentralGetwayServiceBase
    {
        Guest Register(RequestHeader header, string sex, string iDNO, string iDType, string mobile, string name, string password);
    }
}
