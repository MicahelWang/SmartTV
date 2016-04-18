using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel
{
    public static partial class RedisKey
    {
        #region Brand
        public const string CityKey = "Core.City";
        public const string ProvinceKey = "Core.Province";
        public const string CountyKey = "Core.CountyKey";
        #endregion

        #region Hotel
        public const string HoteleKey = "Core.Hotel";
        public const string HotelEntityKey = "Core.HotelEntity";
        public const string TvHotelConfigKey = "Core.TvHotelConfig";
        #endregion Hotel

        #region Brand
        public const string BrandKey = "Core.Brand";
        #endregion

        #region Group
        public const string GroupKey = "Core.Group";
        #endregion Group

        #region Role
        public const string RoleKey = "Core.Role";
        #endregion

        #region Power
        public const string PowerKey = "Core.Power";
        public const string PowerByRoleKey = "Core.PowerByRole";
        public const string MenuByRoleKey = "Core.MenuByRole";
        #endregion

        #region PowerRelation
        public const string PowerRelationKey = "Core.PowerRelation";
        #endregion


        #region Template

        public const string TemplateTypesKey = "Tv.TemplateTypes";

        public const string TemplateElementsKey = "Tv.TemplateElements";

        public const string TemplateAttributesKey = "Tv.TemplateAttributes";

        public const string DocumentElementsKey = "Tv.DocmentElements";

        public const string DocumentOnlyElementsKey = "Tv.DocmentOnlyElements";

        public const string TemplatesKey = "Tv.Templates";

        public const string DocumentAttributesKey = "Tv.DocumentAttributes";

        public const string TemplateViewKey = "Tv.TemplateView";

        public const string TemplatesRootKey = "Tv.Template_";


        #endregion

        #region VOD

        public const string VODOrdersKey = "Tv.VODOrders";
        public const string VODDailyOrdersKey = "Tv.VODDailyOrders";

        #endregion

        #region APP
        public const string AppsKey = "Tv.Apps";

        public const string AppPublishKey = "Tv.AppPublish";

        public const string AppVersionKey = "Tv.AppVersion";

        #endregion


        #region Attachment

        public const string AttachmentDataKey = "YeahTV_AttachmentData";

        #endregion

        #region DeviceTrace
        public const string DeviceTraceDataKey = "YeahTV_DeviceTraceData";
        public const string BackupDeviceDataKey = "YeahTV_BackupDeviceData";
        #endregion
        #region TVCache
        public const string TVChannelDataKey = "YeahTV_TVChannelData";
        public const string HotelTVChannelDataKey = "YeahTV_HotelTVChannelData";
        #endregion
        #region Movie

        public const string MovieSetsKey = "YeahTV_Movies";
        public const string MovieTemplateRelationsKey = "YeahTV_MovieTemplateRelations";
        public const string MovieTemplatesKey = "YeahTV_MovieTemplates";
        public const string HotelMovieTracesKey = "YeahTV_HotelMovieTraces";

        #endregion

        #region Movie and LangResource
        public const string MovieForLocalizeKey = "YeahTV_MovieForLocalize";
        public const string HotelMovieTraceNoTemplateKey = "YeahTV_HotelMovieTraceNoTemplate";
        //public const string LocalizeResourceKey = "YeahTV_LocalizeResource";
        public const string TagKey = "YeahTV_Tag";
        #endregion

        #region HCS

        public const string TaskKey = "HCS_Task";

        public const string HCSConfigKey = "HCS_Config";

        #endregion HCS
        #region HotelPermition
        public const string HotelPermitionKey = "YeahTV_HotelPermition";
        #endregion

        #region DashBoard

        public const string DashBoard_HotelStartPercentage = "DashBoard_HotelStartPercentage";

        public const string DashBoard_HotelMovieIncome = "DashBoard_HotelMovieIncome";

        public const string DashBoard_HotelModuleUsedTime = "DashBoard_HotelModuleUsedTime";

        public const string DashBoard_HotelMovieVodOfDay = "DashBoard_HotelMovieVodOfDay";

        public const string DashBoard_HotelChannelUsedTime = "DashBoard_HotelChannelUsedTime";

        #endregion

        #region 积分

        public const string HotelToken = "YeahTV_HotelToken_";
        #endregion
    }
}
