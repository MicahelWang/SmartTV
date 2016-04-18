namespace YeahTVApiLibrary.Manager
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApiLibrary.EntityFrameworkRepository;
    using YeahTVApi.Common;
    using System.Threading;
    using YeahTVApi.DomainModel.Models.DataModel;

    public class LogManager : ILogManager, IDisposable
    {
        private IBehaviorLogRepertory behaviorLogRepertory;
        private ISystemLogRepertory systemLogRepertory;
        private IMongoLogRepository mongoLogRepository;
        private static Semaphore semaphore = new Semaphore(1, 1);

        public LogManager(IBehaviorLogRepertory behaviorLogRepertory, ISystemLogRepertory systemLogRepertory, IMongoLogRepository mongoLogRepository)
        {
            this.behaviorLogRepertory = behaviorLogRepertory;
            this.systemLogRepertory = systemLogRepertory;
            this.mongoLogRepository = mongoLogRepository;
        }

        public void SaveError(string message, object err, AppType appType, string url = null)
        {
            SaveLog(url, message, err, appType.ToString(), LogType.Error);
        }

        public void SaveError(Exception err, object moreinfo, AppType appType, string url = null)
        {
            SaveLog(url, err, moreinfo, appType.ToString(), LogType.Error);
        }

        public void SaveInfo(string message, object Info, AppType appType, string url = null)
        {
            SaveLog(url, message, Info, appType.ToString(), LogType.Infomation);
        }

        public void SaveWarning(string message, AppType appType, object Info = null, string url = null)
        {
            SaveLog(url, message, Info, appType.ToString(), LogType.Waring);
        }

        public void SaveBehavior(string message, object Info, string appId, string url = null)
        {
            SaveLog(url, message, Info, appId, LogType.UserBehavior);
        }

        public List<SystemLog> SearchSystemLog(LogCriteria criteria)
        {
            return systemLogRepertory.Search(criteria);
        }

        public List<BehaviorLog> SearchBehaviorLog(LogCriteria criteria)
        {
            return behaviorLogRepertory.Search(criteria);
        }

        public void SaveBehavior(List<BehaviorLogRequestNew> behaviorLogRequests, string hotelId, string deviceSerise)
        {
            var task = new Task(() =>
            {
                var logs = new List<BehaviorLog>();
                behaviorLogRequests.ForEach(b =>
                {
                    var log = new BehaviorLog
                    {
                        HotelId = hotelId,
                        CreateTime = DateTime.Now,
                        BehaviorInfo = b.ObjectInfo.ToString(),
                        DeviceSerise = deviceSerise,
                        BehaviorType = b.BehaviorType.ToString(),
                    };

                    logs.Add(log);
                });


                behaviorLogRepertory.Insert(logs);
            });

            task.Start();
        }

        [UnitOfWork]
        public void TestTransaction(string url, object message, object moreInfo, string appIdOrType)
        {
            var behaviorLog = new BehaviorLog
            {
                CreateTime = DateTime.Now,
                BehaviorInfo = message != null ? url + message.ToString() : url,
                BehaviorType = "",
            };

            behaviorLogRepertory.Insert(behaviorLog);

            var systemLog = new SystemLog
            {
                AppType = appIdOrType,
                CreateTime = DateTime.Now,
                MessageType = "",
            };

            systemLogRepertory.Insert(systemLog);
        }

        public void Dispose()
        {
            semaphore.Release(1);
        }

        private void SaveLog(string url, object message, object moreInfo, string appIdOrType, LogType logType)
        {
            var thread = new Thread(new ParameterizedThreadStart((o) =>
            {
                semaphore.WaitOne();

                var mongLog = new MongoLog
                {
                    AppType = appIdOrType,
                    CreateTime = DateTime.Now,
                    MessageInfo = moreInfo != null ? moreInfo.ToString() : string.Empty,
                    MessageEx = message != null ? message.ToString() : string.Empty,
                    MessageType = logType.ToString(),
                    Url = url
                };
                mongoLogRepository.Add(mongLog, null);

                semaphore.Release();
            }));

            thread.Start();
        }

        public void SaveSystemLog(List<SystemLogRequest> systemLogRequests, string appId)
        {
            var task = new Task(() =>
            {
                var logs = new List<SystemLog>();
                systemLogRequests.ForEach(b =>
                {
                    var log = new SystemLog
                    {
                        AppType = b.AppType.ToString(),
                        CreateTime = DateTime.Now,
                        MessageInfo = b.Message,
                        MessageInfoEx = b.MoreInfo,
                        MessageType = b.LogType.ToString()
                    };

                    logs.Add(log);
                });


                systemLogRepertory.Insert(logs);
            });

            task.Start();
        }
        public List<MongoLog> SearchSystemLog(DateTime dateTime)
        {
            return mongoLogRepository.GetAll(dateTime);
        }

        public void SaveBehavior(BehaviorLog behaviorLog)
        {
            behaviorLogRepertory.Insert(behaviorLog);
        }
    }
}