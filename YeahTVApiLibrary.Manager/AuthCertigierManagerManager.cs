using System;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApiLibrary.Infrastructure;
using System.Linq;
using System.Collections;

namespace YeahTVApiLibrary.Manager
{
    public class AuthCertigierManagerManager : IAuthCertigierManagerManager
    {
        private readonly IAuthCertigierManagerRepertory _authCertigier;
        private readonly IUserManager _userManager;
        private readonly IAuthUserDeviceTraceRepertory _userDeviceTrace;

        public AuthCertigierManagerManager(IAuthCertigierManagerRepertory authCertigier, IUserManager userManager, IAuthUserDeviceTraceRepertory userDeviceTrace)
        {
            _authCertigier = authCertigier;
            _userManager = userManager;
            _userDeviceTrace = userDeviceTrace;
        }

        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AuthCertigierManager Authorization(string userId, string password)
        {
            var loginEntity = _userManager.Login(userId, password);
            if (loginEntity == null)
            {
                throw new Exception("用户名或密码错误");
            }
            return Authorization(userId);
        }

        private AuthCertigierManager Authorization(string userId)
        {
            var user = _userManager.GetEntity(userId);
            var authCertigierType = AuthCertigierType.Login.ToInt();
            var expireTime = DateTime.Now.AddHours(24 * 7);
            AuthCertigierManager entity;

            if (_authCertigier.IsExtisCertigier(userId, authCertigierType))
            {
                entity = _authCertigier.GetCertigierByUserId(userId, authCertigierType);
                entity.ExpireTime = expireTime;
                _authCertigier.Update(entity);
            }
            else
            {
                entity = new AuthCertigierManager()
                {
                    Id = Guid.NewGuid().ToString("N").ToUpper(),
                    Token = Guid.NewGuid().ToString("N").ToUpper(),
                    CreateTime = DateTime.Now,
                    Type = authCertigierType,
                    ExpireTime = expireTime,
                    UserName = user.UserName,
                    UserId = user.Id
                };
                _authCertigier.Insert(entity);
            }
            return entity;
        }

        public AuthCertigierManager AuthorizationByUserName(string username, string password)
        {
            var loginEntity = _userManager.Login(
                new LoginModel
                {
                    UserName = username,
                    Password = password
                });
            if (loginEntity == null)
            {
                throw new Exception("用户名或密码错误");
            }
            return Authorization(loginEntity.Id);
        }

        public AuthCertigierManager AuthorizationByPhoneNo(string phoneNo, string password)
        {
            var entity = _userManager.GetAll().Where(p => p.Phone == phoneNo).FirstOrDefault();

            if (entity == null)
            {
                throw new Exception(string.Format("该手机号不存在！！！", phoneNo));
            }
            return Authorization(entity.Id, password);
        }

        public AuthCertigierManager AuthorizationByDeviceNo(string deviceNo, string password)
        {
            var entity = _userDeviceTrace.GetDeviceTrace(deviceNo);

            if (entity == null)
            {
                throw new Exception(string.Format("设备 {0} 未绑定,请联系管理员绑定！！！", deviceNo));
            }
            return Authorization(entity.UserId, password);
        }

        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="token">授权码</param>
        /// <returns></returns>
        public AuthCertigierManager Authentication(string userId, string token)
        {
            var authCertigierType = AuthCertigierType.Login.ToInt();

            var entity = _authCertigier.GetCertigierByUserId(userId, authCertigierType);
            if (entity == null || entity.ExpireTime < DateTime.Now || entity.Token != token)
            {
                throw new Exception("授权信息不存在或已过期,请重新登录！");
            }
            entity.ExpireTime = DateTime.Now.AddHours(4);
            _authCertigier.Update(entity);
            return entity;

        }


        public AuthCertigierManager Authentication(string token)
        {
            var authCertigierType = AuthCertigierType.Login.ToInt();

            var entity = _authCertigier.GetCertigier(token, authCertigierType);
            if (entity == null || entity.ExpireTime < DateTime.Now || entity.Token != token)
            {
                throw new Exception("授权信息已过期,请重新登录！");
            }
            entity.ExpireTime = DateTime.Now.AddHours(4);
            _authCertigier.Update(entity);
            return entity;

        }

        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="username">用户名称</param>
        /// <param name="token">授权码</param>
        /// <returns></returns>
        public AuthCertigierManager AuthenticationByUserName(string username, string token)
        {
            var entity = _userManager.GetLoginByUserName(username);
            if (entity == null)
            {
                throw new Exception("用户信息不存在");
            }
            return Authentication(entity.Id, token);
        }
        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="deviceNo">设备号</param>
        /// <param name="token">授权码</param>
        /// <returns></returns>
        public AuthCertigierManager AuthenticationByDeviceNo(string deviceNo, string token)
        {
            var entity = _userDeviceTrace.GetDeviceTrace(deviceNo);
            if (entity == null)
            {
                throw new Exception("设备未绑定");
            }
            return Authentication(entity.UserId, token);
        }

        public AuthCertigierManager AuthenticationByPhoneNo(string token)
        {
            return Authentication(token);
        }
    }
}
