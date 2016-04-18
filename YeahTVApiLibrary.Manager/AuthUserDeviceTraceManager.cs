using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;
using System.Linq;
using System.Collections.Generic;

namespace YeahTVApiLibrary.Manager
{
    public class AuthUserDeviceTraceManager : IAuthUserDeviceTraceManager
    {
        private IAuthUserDeviceTraceRepertory authUserDeviceTraceRepertory;


        public AuthUserDeviceTraceManager(IAuthUserDeviceTraceRepertory authUserDeviceTraceRepertory)
        {
            this.authUserDeviceTraceRepertory = authUserDeviceTraceRepertory;
        }

        public List<AuthUserDeviceTrace> SearhAuthUserDeviceTrace(AuthUserDeviceTraceCriteria authUserDeviceTraceCriteria)
        {
            return authUserDeviceTraceRepertory.Search(authUserDeviceTraceCriteria);
        }

        public void AddAuthUserDeviceTrace(AuthUserDeviceTrace authUserDeviceTrace)
        {
            var list = authUserDeviceTraceRepertory.Search(new AuthUserDeviceTraceCriteria { DeviceNo = authUserDeviceTrace.DeviceNo });
            if ((list == null || !list.Any()))
            {
                authUserDeviceTraceRepertory.Insert(authUserDeviceTrace);
            }

        }

        public void UpdateAuthUserDeviceTrace(AuthUserDeviceTrace authUserDeviceTrace)
        {
            var authUserDeviceTraceDb = authUserDeviceTraceRepertory.FindByKey(authUserDeviceTrace.Id);
            authUserDeviceTraceDb.UserId = authUserDeviceTrace.UserId;
            authUserDeviceTraceDb.DeviceNo = authUserDeviceTrace.DeviceNo;
            authUserDeviceTraceDb.LastVisitTime = authUserDeviceTrace.LastVisitTime;
            authUserDeviceTraceDb.BindTime = authUserDeviceTrace.BindTime;
            
            authUserDeviceTraceRepertory.Update(authUserDeviceTraceDb);
        }

        public void DeleteAuthUserDeviceTrace(AuthUserDeviceTrace authUserDeviceTrace)
        {
            authUserDeviceTraceRepertory.Delete(a => a.Id == authUserDeviceTrace.Id);
        }

        public AuthUserDeviceTrace GetEntity(string id)
        {
            return authUserDeviceTraceRepertory.FindByKey(id);
        }

        public AuthUserDeviceTrace GetAuthUserDeviceTrace(AuthUserDeviceTraceCriteria criteria)
        {
            return authUserDeviceTraceRepertory.Search(criteria).FirstOrDefault();
        }

    }
}
