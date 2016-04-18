using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;

namespace YeahWebApi.MongoRepository
{
    public class BaseRepository<Tentity> where Tentity : class
    {
        
        public static MongoContext Context { get; set; }

        public MongoCollection<Tentity> Entities
        {
            get
            {
                return Context.EntityCollection.SingleOrDefault(m => m.Key == typeof(Tentity)).Value as MongoCollection<Tentity>;
            }
        }

        public List<Tentity> GetAll(DateTime? logDate = null)
        {
            if (logDate.HasValue)
                Context = new MongoContext(logDate.Value);
            else
                Context = new MongoContext();

            return Entities.AsQueryable().ToList();
        }

        public void Add(Tentity entity, DateTime? logDate = null)
        {
            if (logDate.HasValue)
                Context = new MongoContext(logDate.Value);
            else
                Context = new MongoContext();

            Entities.Insert(entity);
        }

        public void Update(Tentity entity, DateTime? logDate = null)
        {
            if (logDate.HasValue)
                Context = new MongoContext(logDate.Value);
            else
                Context = new MongoContext();

            Entities.Save(entity);
        }      
    }
}
