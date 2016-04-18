namespace YeahTVApi.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Infrastructure;
    using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    public class TVAppVersionRepertory : BaseRepertory<AppVersion, string>, ITVAppVersionRepertory
    {

        public string GetLastestAppVersion(string appId, string seriesCode)
        {


            var result = base.Context.Database.SqlQuery<string>(@"SELECT TOP 1 t2.APP_URL  AS AppUrl FROM APP_PUBLISH AS  t1
                                                INNER JOIN TV_VERSION AS  t2
                                                ON  t1.VERSION_CODE=t2.VERSION_CODE   where T1.ACTIVE=1 AND T2.ACTIVE=1
                                                AND PUBLISH_DATE<=dbo.FUNC_TO_DATE_TIME_INTEGER(getdate())
                                                AND T2.APP_ID=@APP_ID
                                                AND ( SERIES_CODE=@SERIES_CODE 
                                                OR (SERIES_CODE IS NULL ))
                                                ORDER BY t2.VERSION_CODE DESC",
                new SqlParameter { ParameterName = "APP_ID", Value = appId },
                new SqlParameter { ParameterName = "SERIES_CODE", Value = seriesCode }
               ).AsQueryable().SingleOrDefault();   
            return result;

        }


        public override List<AppVersion> Search(BaseSearchCriteria searchCriteria)
        {
            throw new System.NotImplementedException();
        }


    }
}
