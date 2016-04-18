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

    public class MongoLogRepository : BaseRepository<MongoLog>, IMongoLogRepository
    {
        public void RemoveMongoLog(MongoLog log, DateTime? logDate = null)
        {
            if (logDate.HasValue)
                Context = new MongoContext(logDate.Value);
            else
                Context = new MongoContext();
            
            var query = Query<MongoLog>.EQ(e => e.Id, log.Id);
            Entities.Remove(query);
        }

        public List<MongoLog> Search(MongoCriteria mongoCriteria)
        {
            if (mongoCriteria.LogDate.HasValue)
                Context = new MongoContext(mongoCriteria.LogDate.Value);
            else
                Context = new MongoContext();

            var query = base.Entities.AsQueryable();

            if (mongoCriteria.AppType.HasValue)
                query = query.Where(q => q.AppType == mongoCriteria.AppType.Value.ToString());

            if (!string.IsNullOrEmpty(mongoCriteria.MessageEx))
                query = query.Where(q => q.MessageEx.Contains(mongoCriteria.MessageEx));

            if (mongoCriteria.LogType.HasValue)
                query = query.Where(q => q.MessageType == mongoCriteria.LogType.Value.ToString());

            if (!string.IsNullOrEmpty(mongoCriteria.Message))
                query = query.Where(q => q.MessageInfo.Contains(mongoCriteria.Message));

            if (!string.IsNullOrEmpty(mongoCriteria.Url))
                query = query.Where(q => q.Url.Contains(mongoCriteria.Url));

            if (!mongoCriteria.OrderAsc)
                query = query.OrderByDescending(q => q.CreateTime);
            
            if (mongoCriteria.NeedPaging)
            {
                mongoCriteria.TotalCount = query.Count();
                query = query.Page(mongoCriteria.PageSize, mongoCriteria.Page);
            }

            return query.ToList();
        }
        public MongoLog GetById(string id)
        {
            var query = base.Entities.AsQueryable();
            ObjectId objectId=new ObjectId(id);
                query = query.Where(q => q.Id ==objectId);
                return query.Single();
        }
    }
}
