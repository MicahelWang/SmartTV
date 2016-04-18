namespace HZTVApi.Business
{
    using HZTVApi.DomainModel.Models;
    using HZTVApi.DomainModel.SearchCriteria;
    using HZTVApi.Infrastructure;
    using System.Collections.Generic;

    public class TVAppsManager : ITVAppsManager
    {
        private ITVAppsRepertory appsRepertory;

        public TVAppsManager(ITVAppsRepertory appsRepertory)
        {
            this.appsRepertory = appsRepertory;
        }

        public List<TVApps> GetStroeApps()
        {
            return appsRepertory.Search(new TVAppsModelCriteria { ShowInStroe = true });
        }
    }
}
