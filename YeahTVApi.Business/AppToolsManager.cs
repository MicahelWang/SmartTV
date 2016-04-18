namespace HZTVApi.Manager
{
    using HZTVApi.Entity.AppTools;
    using HZTVApi.Infrastructure;

    /// <summary>
    /// appTools
    /// </summary>
    public class AppToolsManager : IAppToolsManager
    {
        private IAppToolService appToolService;

        public AppToolsManager(IAppToolService appToolService)
        {
            this.appToolService = appToolService;
        }

        /// <summary>
        /// pms登录返回
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public EmployeeEntity Auth(string userName, string password, string hotelId)
        {
            return appToolService.Auth(userName, password, hotelId);
        }
    }
}
