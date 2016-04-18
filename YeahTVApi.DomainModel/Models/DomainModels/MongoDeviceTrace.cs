namespace YeahTVApi.DomainModel
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MongoDeviceTrace
    {
        public ObjectId Id { get; set; }
        public string DeviceSeries { get; set; }
        public string HotelId { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)] 
        public DateTime VisitTime { get; set; }
    }
}
