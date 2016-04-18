namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.SearchCriteria;
    using System;
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Common;

    public interface ILogManager : IDisposable
    {
        List<SystemLog> SearchSystemLog(LogCriteria criteria);

        List<BehaviorLog> SearchBehaviorLog(LogCriteria criteria);

        void SaveError(string message, object err, AppType appType, string url = null);

        void SaveError(Exception err, object moreinfo, AppType appType, string url = null);

        void SaveInfo(string message, object Info, AppType appType, string url = null);

        void SaveWarning(string message, AppType appType, object Info = null, string url = null);

        void SaveBehavior(string message, object Info, string appId, string url = null);

        void SaveBehavior(List<BehaviorLogRequestNew> behaviorLogRequests, string hotelId, string deviceSerise);

        [UnitOfWork]
        void TestTransaction(string url, object message, object moreInfo, string appIdOrType);

        void SaveSystemLog(List<YeahTVApi.DomainModel.Models.DataModel.SystemLogRequest> systemLogRequests, string appId);

        void SaveBehavior(BehaviorLog behaviorLog);
    }
}
