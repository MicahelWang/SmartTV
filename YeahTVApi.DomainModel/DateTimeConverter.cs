using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class TimestampDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(String.Format("日期格式错误,got {0}.", reader.TokenType));
            }
            var ticks = (long)reader.Value;
            var date = new DateTime(1970, 1, 1);
            date = date.AddSeconds(ticks);
            return date;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var epoc = new DateTime(1970, 1, 1);
                var delta = ((DateTime)value) - epoc;
                if (delta.TotalSeconds < 0)
                {
                    throw new ArgumentOutOfRangeException("时间格式错误.1");
                }
                ticks = (long)delta.TotalSeconds;
            }
            else
            {
                throw new Exception("时间格式错误.2");
            }
            writer.WriteValue(ticks);
        }
    }

    public class StringDateTimeConverter : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String)
            {
                throw new Exception(String.Format("日期格式错误,got {0}.", reader.TokenType));
            }

            var date = DateTime.ParseExact(reader.Value.ToString(), "yyyy-MM-dd HH:mm:ss", null);

            return date;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string date;

            if (value is DateTime)
            {
                date = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                throw new Exception("时间格式错误.2");
            }
            writer.WriteValue(date);
        }
    }

    public class TimestampConverterToString : DateTimeConverterBase
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.Null && reader.TokenType != JsonToken.Integer)
            {
                throw new Exception(String.Format("日期格式错误,got {0}.", reader.TokenType));
            }
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            var ticks = (long)reader.Value;
            var date = new DateTime(1970, 1, 1);
            date = date.AddSeconds(ticks);
            return date.ToShortDateString();
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            long ticks;
            if (value is DateTime)
            {
                var epoc = new DateTime(1970, 1, 1);
                var delta = ((DateTime)value) - epoc;
                if (delta.TotalSeconds < 0)
                {
                    throw new ArgumentOutOfRangeException("时间格式错误.1");
                }
                ticks = (long)delta.TotalSeconds;
            }
            else
            {
                throw new Exception("时间格式错误.2");
            }
            writer.WriteValue(ticks);
        }
    }
}
