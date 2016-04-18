namespace YeahTVApiLibrary.Manager
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MongoLogManager : IMongoLogManager
    {
        private IMongoLogRepository mongoLogRepository;

        public MongoLogManager(IMongoLogRepository mongoLogRepository)
        {
            this.mongoLogRepository = mongoLogRepository;
        }

        public void AddLog(MongoLog log, DateTime? logDate = null)
        {
            mongoLogRepository.Add(log, logDate);
        }

        public void RemoveLog(MongoLog log, DateTime? logDate = null)
        {
            mongoLogRepository.RemoveMongoLog(log, logDate);
        }

        public void EditLog(MongoLog log, DateTime? logDate = null)
        {
            mongoLogRepository.Update(log, logDate);
        }

        public List<MongoLog> Search(MongoCriteria criteria)
        {
            return mongoLogRepository.Search(criteria);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="err"></param>
        public void SaveError(
            string message,
            object err,
            AppType appType, string url = null)
        {
            SaveInfo(url, message, err, LogType.Error, appType);
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="err"></param>
        public void SaveError(
            Exception err,
            object moreinfo,
            AppType appType,string url = null)
        {
            SaveInfo(url, err.Message + err.StackTrace, moreinfo, LogType.Error, appType);
        }


        /// <summary>
        /// 记录系统消息相关内容
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Info"></param>
        public void SaveInfo(
            string message,
            object Info,
            AppType appType, string url = null)
        {
            SaveInfo(url, message, Info, LogType.Infomation, appType);
        }

        public void SaveWarning(
            string message,
            AppType appType,
            object Info = null, string url = null)
        {
            SaveInfo(url, message, Info, LogType.Waring, appType);
        }

        private void SaveInfo(string Url,
            object message,
            object moreInfo,
            LogType mogoLogType,
            AppType appType)
        {
            var task = new Task(() =>
            {
                var log = new MongoLog
                {
                    AppType = appType.ToString(),
                    CreateTime = DateTime.Now,
                    MessageInfo = message != null ? message.ToString() : string.Empty,
                    MessageEx = moreInfo != null ? moreInfo.ToString() : string.Empty,
                    MessageType = mogoLogType.ToString(),
                    Url = Url
                };

                mongoLogRepository.Add(log, null);
            });

            task.Start();
        }

        public MongoLog GetById(string id)
        {
           return  mongoLogRepository.GetById(id);
        }
    }
}


