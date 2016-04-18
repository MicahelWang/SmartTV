using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YeahTvHcsApi.Common.HttpParameterBinding
{
    public class JsonDataConverter<T> : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string valueString = value as string;

                JObject json = JObject.Parse(valueString);
                dynamic jsonObject = json;
                //string a = jsonObject.biz_no;
                //string b = jsonObject.biz_type;
                //string c = jsonObject.status;
                //string d = jsonObject.err_msg;
                //object o = jsonObject.ToObject<IDictionary<string, T>>();

                object oo = JsonConvert.DeserializeObject<T>(valueString);
                object o = jsonObject.ToObject<object>();

                return oo;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}