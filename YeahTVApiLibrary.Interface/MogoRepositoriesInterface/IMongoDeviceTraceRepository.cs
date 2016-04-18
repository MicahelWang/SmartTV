namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using System;
    using System.Collections.Generic;

    public interface IMongoDeviceTraceRepository
    {
        List<MongoDeviceTrace> GetAll(DateTime? logDate = null);
        void Add(MongoDeviceTrace log, DateTime? logDate = null);
        void Update(MongoDeviceTrace log, DateTime? logDate = null);
        List<MongoDeviceTrace> Search(MongoDeviceCriteria mongoCriteria);
        MongoDeviceTrace GetById(string id);
    }
}
