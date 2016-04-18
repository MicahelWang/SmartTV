using System.Web.Mvc;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.Common;
using System.Web.Http;
using Unity.WebApi;
using System.Web.Api;
using System.Collections.Generic;

namespace YeahAppCentre.Web.Utility
{
    public static class UtilityHelper
    {
        public static string GetGroupName(string id)
        {
            var manager = GetServiceBothMvcApi<IGroupManager>();
            var group = manager.GetGroup(id);
            return group == null ? "" : group.GroupName;
        }

        public static string GetBrandName(string id)
        {
            var manager = GetServiceBothMvcApi<IBrandManager>();
            var brand = manager.GetBrand(id);
            return brand == null ? "" : brand.BrandName;
        }
        public static string GetHotelName(string id)
        {
            var manager = GetServiceBothMvcApi<IHotelManager>();
            var hotel = manager.GetHotel(id);
            return hotel == null ? "" : hotel.HotelName;
        }

        public static string GetRoleName(string id)
        {
            var manager = GetServiceBothMvcApi<IRoleManager>();
            var role = manager.GetEntity(id);
            return role == null ? "" : role.RoleName;
        }

        public static string GetAppName(string id)
        {
            var manager = GetServiceBothMvcApi<IAppLibraryManager>();
            var app = manager.GetAppByAppId(id);
            return app == null ? "" : app.Name;
        }

        public static string GetAttachmentPath(int id)
        {
            var manager = GetServiceBothMvcApi<ISysAttachmentManager>();
            var model = manager.GetById(id);
            return model == null ? "" : model.FilePath;
        }
        public static MvcHtmlString GetHotelName(this HtmlHelper htmlHelper, string hotelId)
        {
            var manager = GetServiceBothMvcApi<IHotelManager>();
            var model = manager.GetHotel(hotelId);
            if (model != null)
            {
                return new MvcHtmlString(model.HotelName);
            }
            return new MvcHtmlString("没有此酒店");

        }
        public static MvcHtmlString GetAttachmentFullPath(int id)
        {
            var manager = GetServiceBothMvcApi<ISysAttachmentManager>();
            var constantmanager = GetServiceBothMvcApi<IConstantSystemConfigManager>();
            ;
            var model = manager.GetById(id);
            return MvcHtmlString.Create(model == null ? "" : constantmanager.ResourceSiteAddress + model.FilePath);
        }

        public static object RequestTemplateByRootName(string templateId, string getTemplateUrl, TemplateRootType templateRootType)
        {
            var requestTemplateUrl = string.Format(getTemplateUrl, templateId, templateRootType);

            return GetServiceBothMvcApi<IRequestApiService>().Get(requestTemplateUrl).JsonStringToObj<object>();
        }
        public static string GetAttachmentResourceSiteAddressPath()
        {
            var constantmanager = DependencyResolver.Current.GetService<IConstantSystemConfigManager>();
            return  constantmanager.ResourceSiteAddress ;
        }

        public static TService GetServiceBothMvcApi<TService>() where TService : class
        {
            var service = DependencyResolver.Current.GetService<TService>();
            return service == null ? GlobalConfiguration.Configuration.DependencyResolver.GetService<TService>() : service;
        }

        public static IEnumerable<TService> GetServicesBothMvcApi<TService>() where TService : class
        {
            var services = DependencyResolver.Current.GetServices<TService>();
            return services == null ? GlobalConfiguration.Configuration.DependencyResolver.GetServices<TService>() : services;
        }
    }
}
