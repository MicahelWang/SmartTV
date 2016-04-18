namespace YeahTVApi.DomainModel.Mapping
{
    using YeahTVApi.DomainModel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MapApp
    {
        public static List<App> ToApp(this List<AppPublish> appPublishs, string deviceSeries)
        {
            var apps = new List<App>();
            
            appPublishs
                .Where(a=>a.AppVersion.App.ShowInStroe.HasValue && a.AppVersion.App.ShowInStroe.Value)
                .GroupBy(q=>q.Id).ToList()
                .ForEach(group => 
            {
                var selectPublish = group.OrderByDescending(o => o.VersionCode)
                     .Where(o => o.PublishDate<= DateTime.Now)
                     .FirstOrDefault();
                
                apps.Add(new App
                    {
                        AppName = selectPublish.AppVersion.App.Name,
                        IconUrl = selectPublish.AppVersion.App.IconUrl,
                        Info = selectPublish.AppVersion.Description,
                        PackageName = selectPublish.AppVersion.App.PackageName,
                        PackageUrl = selectPublish.AppVersion.AppUrl,
                        Version = selectPublish.VersionCode.ToString()
                    });
            });

            return apps;
        }
    }
}
