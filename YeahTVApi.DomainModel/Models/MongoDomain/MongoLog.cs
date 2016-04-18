namespace YeahTVApi.DomainModel
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MongoLog
    {
        public ObjectId Id { get; set; }

        public string Url { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogMessageInfo")]
        public string MessageInfo { get; set; }
          [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogMessageInfoEx")]
        public string MessageEx { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogMessageType")]
        public string MessageType { get; set; }
         [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogCreateTime")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] 
        public DateTime CreateTime { get; set; }
          [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySystemLogAppType")]
        public string AppType { get; set; }
    }
}
