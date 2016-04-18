namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.SearchCriteria;
    using System;
    using System.Collections.Generic;

    public interface IMongoLogManager
    {
        void AddLog(MongoLog log, DateTime? logDate = null);

        void RemoveLog(MongoLog log, DateTime? logDate = null);

        void EditLog(MongoLog log, DateTime? logDate = null);

        List<MongoLog> Search(MongoCriteria criteria);

        void SaveError(string message, object err, AppType appType, string url = null);

        void SaveError(Exception err, object moreinfo, AppType appType, string url = null);

        void SaveInfo(string message, object Info, AppType appType, string url = null);

        void SaveWarning(string message, AppType appType, object Info = null, string url = null);
        MongoLog GetById(string id);
    }
}
