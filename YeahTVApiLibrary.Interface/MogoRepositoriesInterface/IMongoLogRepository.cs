namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using System;
    using System.Collections.Generic;

    public interface IMongoLogRepository
    {
        List<MongoLog> GetAll(DateTime? logDate = null);
        void Add(MongoLog log, DateTime? logDate = null);
        void Update(MongoLog log, DateTime? logDate = null);
        void RemoveMongoLog(MongoLog log, DateTime? logDate = null);
        List<MongoLog> Search(MongoCriteria mongoCriteria);
        MongoLog GetById(string id);
    }
}
