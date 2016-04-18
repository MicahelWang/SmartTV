namespace YeahWebApi.MongoRepository
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using MongoDB.Driver.Builders;
    using MongoDB.Driver.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;

    public class MongoDeviceTraceRepository : BaseRepository<MongoDeviceTrace>, IMongoDeviceTraceRepository
    {
        public List<MongoDeviceTrace> Search(MongoDeviceCriteria mongoCriteria)
        {
                Context = new MongoContext();

            var query = base.Entities.AsQueryable();

            if (mongoCriteria.VisitTimeBegin.HasValue && mongoCriteria.VisitTimeEnd.HasValue)
                query = query.Where(q => q.VisitTime < mongoCriteria.VisitTimeEnd && q.VisitTime > mongoCriteria.VisitTimeBegin);

            if (mongoCriteria.NeedPaging)
            {
                mongoCriteria.TotalCount = query.Count();
                query = query.Page(mongoCriteria.PageSize, mongoCriteria.Page);
            }

            return query.ToList();
        }
        public MongoDeviceTrace GetById(string id)
        {
            Context = new MongoContext();
            var query = base.Entities.AsQueryable();
            ObjectId objectId = new ObjectId(id);
            query = query.Where(q => q.Id == objectId);
            return query.Single();
        }
    }
}
