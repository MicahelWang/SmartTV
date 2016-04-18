namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;
    using System.Collections.Generic;
    using YeahTVApiLibrary.Infrastructure;

    public interface IAuthCertigierManagerManager
    {
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        AuthCertigierManager Authorization(string userId, string password);

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        AuthCertigierManager AuthorizationByUserName(string username, string password);


        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        AuthCertigierManager AuthorizationByDeviceNo(string deviceNo, string password);

        AuthCertigierManager AuthorizationByPhoneNo(string phoneNo, string password);
        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="token">授权码</param>
        /// <returns></returns>
        AuthCertigierManager Authentication(string userId, string token);

        AuthCertigierManager Authentication(string token);
        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="token">授权码</param>
        /// <returns></returns>
        AuthCertigierManager AuthenticationByUserName(string username, string token);
        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="deviceNo">设备编号</param>
        /// <param name="token">授权码</param>
        /// <returns></returns>
        AuthCertigierManager AuthenticationByDeviceNo(string deviceNo, string token);

        AuthCertigierManager AuthenticationByPhoneNo(string token);
    }
}
