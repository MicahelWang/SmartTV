namespace YeahTVApiLibrary.Filter
{
    using System;
    using System.Web.Mvc;
    using Newtonsoft.Json;

    /// <summary>
    /// This lets us return a NewtonSoft Json object as an ActionResult
    /// http://james.newtonking.com/archive/2008/10/16/asp-net-mvc-and-json-net
    /// </summary>
    public class CustomJsonResult : JsonResult
    {
        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }

        public CustomJsonResult()
        {
            SerializerSettings = new JsonSerializerSettings();
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;
            response.ContentType = !string.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            if (ContentEncoding != null)
                response.ContentEncoding = ContentEncoding;

            if (Data != null)
            {
                var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };
                var serializer = JsonSerializer.Create(SerializerSettings);
                serializer.Serialize(writer, Data);
                writer.Flush();
            }
        }
    }
}