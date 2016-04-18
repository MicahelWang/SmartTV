namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;

    public interface IAuthUserDeviceTraceRepertory : IBsaeRepertory<AuthUserDeviceTrace>
    {
        AuthUserDeviceTrace GetDeviceTrace(string deviceNo);
    }
}
