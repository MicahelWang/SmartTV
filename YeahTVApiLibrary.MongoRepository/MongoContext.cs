using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel;
using System.Configuration;
using YeahTVApi.Common;

namespace YeahWebApi.MongoRepository
{
    public class MongoContext
    {
        public MongoDatabase Database { get; set; }

        public Dictionary<Type, MongoCollection> EntityCollection { get;set;}

        public string LogsDate { get; set; }

        public MongoContext()
        {
            var connectionString = PubFun.GetAppSetting(Constant.MongoConnectionStringName);
            var mongoDbName = PubFun.GetAppSetting(Constant.MongoDbNameString);
           if (string.IsNullOrEmpty(connectionString))
               throw new Exception("链接字符串 MongoConnectionString 不存在");
           if (string.IsNullOrEmpty(mongoDbName))
               throw new Exception("链接字符串 mongoDbName 不存在");
           LogsDate = DateTime.Now.GetLogsDate();

           SetAllCollection(connectionString, mongoDbName);
        }

        public MongoContext(DateTime LogsDate)
        {
            var connectionString = PubFun.GetAppSetting(Constant.MongoConnectionStringName);
            var mongoDbName = PubFun.GetAppSetting(Constant.MongoDbNameString);
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("链接字符串 MongoConnectionString 不存在");
            if (string.IsNullOrEmpty(mongoDbName))
                throw new Exception("链接字符串 mongoDbName 不存在");
            this.LogsDate = LogsDate.GetLogsDate();

            SetAllCollection(connectionString, mongoDbName);
        }

        public MongoContext(string connectionStringName, DateTime LogsDate)
        {
            var connectionString = PubFun.GetAppSetting(connectionStringName);
            var mongoDbName = PubFun.GetAppSetting(Constant.MongoDbNameString);
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("链接字符串 MongoConnectionString 不存在");
            if (string.IsNullOrEmpty(mongoDbName))
                throw new Exception("链接字符串 mongoDbName 不存在");
            this.LogsDate = LogsDate.GetLogsDate();              

            SetAllCollection(connectionString,mongoDbName);
        }

        public MongoCollection<MongoLog> MongoLogs { get; set; }
        public MongoCollection<MongoDeviceTrace> MongoDeviceTraces { get; set; }

        private void SetAllCollection(string connectionString, string mongoDbName)
        {
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            Database = server.GetDatabase(mongoDbName);

            MongoLogs = Database.GetCollection<MongoLog>("MongoLogs" + LogsDate);
            MongoDeviceTraces = Database.GetCollection<MongoDeviceTrace>("MongoDeviceTraces");

            EntityCollection = new Dictionary<Type, MongoCollection>();

            EntityCollection.Add(typeof(MongoLog), MongoLogs);
            EntityCollection.Add(typeof(MongoDeviceTrace), MongoDeviceTraces);
        }
    }
}
