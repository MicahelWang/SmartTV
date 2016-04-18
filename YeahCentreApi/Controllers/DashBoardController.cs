using System;
using System.Collections.Generic;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Infrastructure;
using YeahCenter.Infrastructure;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class DashBoardController : ApiController
    {
        private readonly IMongoDeviceTraceManager _mongoDeviceTraceManager;
        private readonly IVODOrderManager _vodOrderManager;
        private readonly IBehaviorLogManager _behaviorLogManager;
        private readonly IDeviceTraceLibraryManager _deviceTraceLibraryManager;
        private readonly IBackupDeviceManager _backupDeviceManager;
        private readonly IHotelPermitionManager _hotelPermitionManager;
        private readonly IDashBoardManager _dashBoardManager;
        private readonly IHotelManager _hotelManager;
        private readonly IMovieForLocalizeWrapperFacade _movieManager;
        private readonly ITVChannelManager _tvChannelManager;
        private readonly IBrandManager _brandManager;
        private readonly IConstantSystemConfigManager _constantSystemConfigManager;

        public DashBoardController(IMongoDeviceTraceManager mongoDeviceTraceManager
            , IVODOrderManager vodOrderManager
            , IMovieForLocalizeWrapperFacade movieManager
            , ITVChannelManager tvChannelManager
            , IBehaviorLogManager behaviorLogManager
            , IDeviceTraceLibraryManager deviceTraceLibraryManager
            , IBackupDeviceManager backupDeviceManager
            , IHotelPermitionManager hotelPermitionManager
            , IDashBoardManager dashBoardManager
            , IHotelManager hotelManager
            , IBrandManager brandManager
            , IConstantSystemConfigManager constantSystemConfigManager)
        {
            _mongoDeviceTraceManager = mongoDeviceTraceManager;
            _vodOrderManager = vodOrderManager;
            _behaviorLogManager = behaviorLogManager;
            _movieManager = movieManager;
            _tvChannelManager = tvChannelManager;
            this._deviceTraceLibraryManager = deviceTraceLibraryManager;
            this._backupDeviceManager = backupDeviceManager;
            _hotelPermitionManager = hotelPermitionManager;
            _dashBoardManager = dashBoardManager;
            _hotelManager = hotelManager;
            _brandManager = brandManager;
            _constantSystemConfigManager = constantSystemConfigManager;

        }


        #region 刷新缓存
        [HttpGet]
        public HttpResponseMessage RefreshCache()
        {
            string errorMessage = "";
            try
            {

                //开机率缓存
                _mongoDeviceTraceManager.RefreshHotelStartPercentage();

                //VOD收益趋势 及 排行TOP10
                _vodOrderManager.RefreshHotelMovieIncome();

                //模块使用时长
                _behaviorLogManager.RefreshBehaviorLogDashBoard();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return new HttpResponseMessage { Content = new StringContent((string.IsNullOrWhiteSpace(errorMessage) ? "success" : errorMessage)) };
        }

        #endregion
    }
}