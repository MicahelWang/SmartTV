namespace YeahTVApiLibrary.Filter
{
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Newtonsoft.Json;
    using System.Web.Http;

    public class JsonHandlerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var parameters = filterContext.ActionDescriptor.GetParameters();
            if (!parameters.Any())
            {
                return;
            }

            var fromUriParameters = parameters.Where(p =>
                p.GetCustomAttributes(typeof(FromUriAttribute), true).Any());
            var fromBodyParameters = parameters.Except(fromUriParameters);
            var request = filterContext.HttpContext.Request;

            if (request.ContentType.Contains("application/json") && fromBodyParameters.Any())
            {
                string inputContent;
                request.InputStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(request.InputStream))
                {
                    inputContent = sr.ReadToEnd();
                }

                parameters.ToList().ForEach(p =>
                {
                    if (!p.ParameterType.IsValueType && p.ParameterType != typeof(string))
                    {
                        var obj = JsonConvert.DeserializeObject(inputContent, p.ParameterType);
                        filterContext.ActionParameters[p.ParameterName] = obj;
                    }
                });
            }

            if (request.QueryString.HasKeys() && fromUriParameters.Any())
            {
                fromUriParameters.ToList().ForEach(p =>
                {
                    if (!p.ParameterType.IsValueType && p.ParameterType != typeof(string))
                    {
                        var obj = DeserializeObjectFromQueryString(request.QueryString, p.ParameterType);
                        filterContext.ActionParameters[p.ParameterName] = obj;
                    }
                });
            }
        }

        private static object DeserializeObjectFromQueryString(NameValueCollection queryString, Type type)
        {
            var obj = Activator.CreateInstance(type);
            var props = type.GetProperties().Where(p => p.CanWrite);
            var nameProps = props.Select(p =>
            {
                var propName = p.Name;
                var jsonPropAttr = p.GetCustomAttributes(typeof(JsonPropertyAttribute), true)
                        .OfType<JsonPropertyAttribute>().FirstOrDefault();
                if (jsonPropAttr != null)
                {
                    propName = jsonPropAttr.PropertyName;
                }

                return new
                {
                    Name = propName,
                    PropInfo = p
                };
            });
            foreach (string key in queryString.Keys)
            {
                var prop = nameProps
                        .Where(p => string.Equals(p.Name, key, StringComparison.OrdinalIgnoreCase))
                        .Select(p => p.PropInfo)
                        .FirstOrDefault();

                if (prop != null)
                {
                    var strVal = queryString[key];
                    if (strVal != null)
                    {
                        var typeConverter = TypeDescriptor.GetConverter(prop.PropertyType);
                        var val = typeConverter.ConvertFromString(strVal);
                        prop.SetValue(obj, val,null);
                    }
                }
            }

            return obj;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var jsonResult = filterContext.Result as JsonResult;

            if (jsonResult != null)
            {
                filterContext.Result = new CustomJsonResult
                {
                    ContentEncoding = jsonResult.ContentEncoding,
                    ContentType = jsonResult.ContentType,
                    Data = jsonResult.Data,
                    JsonRequestBehavior = jsonResult.JsonRequestBehavior
                };
            }

            base.OnActionExecuted(filterContext);
        }
    }
}