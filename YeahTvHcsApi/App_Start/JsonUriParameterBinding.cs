using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using YeahTVApi.Common;
/// <summary>
/// Reads the Request body into a string/byte[] and
/// assigns it to the parameter bound.
/// 
/// Should only be used with a single parameter on
/// a Web API method using the [NakedBody] attribute
/// </summary>
using System.Web.Http.Controllers;
using System.Web.ModelBinding;
using System.Net.Http.Formatting;
using YeahTvHcsApi.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace YeahTvHcsApi
{
    public class JsonUriParameterBinding : HttpParameterBinding
    {
        public JsonUriParameterBinding(HttpParameterDescriptor descriptor)
            : base(descriptor)
        {

        }


        public override Task ExecuteBindingAsync(System.Web.Http.Metadata.ModelMetadataProvider metadataProvider,
                                                    HttpActionContext actionContext,
                                                    CancellationToken cancellationToken)
        {
            var binding = actionContext
                .ActionDescriptor
                .ActionBinding;

            if (binding.ParameterBindings.Length > 1 ||
                actionContext.Request.Method == HttpMethod.Get)
                return EmptyTask.Start();

            var type = binding
                        .ParameterBindings[0]
                        .Descriptor.ParameterType;

            if (type == typeof(string))
            {
                return actionContext.Request.Content
                        .ReadAsStringAsync()
                        .ContinueWith((task) =>
                        {
                            var stringResult = task.Result;
                            SetValue(actionContext, stringResult);
                        });
            }
            else if (type.FullName.StartsWith("YeahTvHcsApi.ViewModels.PostParameters"))
            {
                return actionContext.Request.Content
                    .ReadAsStringAsync()
                    .ContinueWith((task) =>
                    {
                        var result = type.Assembly.CreateInstance(type.FullName);

                        foreach (var data in task.Result.Split('&'))
                        {
                            if (!string.IsNullOrEmpty(data))
                            {
                                var dataList = data.Split('=');

                                var property = result.GetType().GetProperties()
                                    .Where(p => p.CustomAttributes.Any(c => c.ConstructorArguments.Any(v => v.Value.Equals(dataList[0]))))
                                    .FirstOrDefault();

                                object vlaue = dataList[1];

                                if (property.Name.Equals("Data"))
                                    vlaue = dataList[1].JsonStringToObj(property.PropertyType);

                                property.SetValue(result, vlaue, null);
                            }
                        }

                        SetValue(actionContext, result);
                    });
            }

            throw new InvalidOperationException("Only string and YeahTvHcsApi.ViewModels.PostParameters are supported for [JsonBodyParameter] parameters");
        }

        public override bool WillReadBody
        {
            get
            {
                return true;
            }
        }


    }

    /// <summary>
    /// A do nothing task that can be returned
    /// from functions that require task results
    /// when there's nothing to do.
    /// 
    /// This essentially returns a completed task
    /// with an empty value structure result.
    /// </summary>
    public class EmptyTask
    {
        public static Task Start()
        {
            var taskSource = new TaskCompletionSource<AsyncVoid>();
            taskSource.SetResult(default(AsyncVoid));
            return taskSource.Task as Task;
        }

        private struct AsyncVoid
        {
        }
    }

    /// <summary>
    /// An attribute that captures the entire content body and stores it
    /// into the parameter of type string or byte[].
    /// </summary>
    /// <remarks>
    /// The parameter marked up with this attribute should be the only parameter as it reads the
    /// entire request body and assigns it to that parameter.    
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class JsonUriAttribute : ParameterBindingAttribute
    {
        public override HttpParameterBinding GetBinding(HttpParameterDescriptor parameter)
        {
            if (parameter == null)
                throw new ArgumentException("Invalid parameter");

            return new JsonUriParameterBinding(parameter);
        }
    }
}