namespace HZTVApi.Manager
{
    using HZTVApi.Infrastructure;
    using HZTVApi.Common;
    using HZTVApi.Entity;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public class GuestManager : IGuestManager
    {
        private IRedisCacheManager redisCacheManager;
        private IGetGuestInfoService guestInfoService;

        public GuestManager(
            IRedisCacheManager redisCacheManager,
            IGetGuestInfoService guestInfoService)
        {
            this.redisCacheManager = redisCacheManager;
            this.guestInfoService = guestInfoService;
        }

        /// <summary>
        /// 用户注销,该动作会记录用户注销时间,以及清空用户ID
        /// </summary>
        /// <param name="DEVICE_SERIES">设备号</param>
        /// <param name="PLATFORM">平台号</param>
        /// <returns></returns>
        public bool Logout(string DEVICE_SERIES, string PLATFORM, string APP_VERSION)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("DEVICE_SERIES", DEVICE_SERIES);
            parameters.Add("PLATFORM", PLATFORM);
            parameters.Add("APP_VERSION", APP_VERSION);
            return DBHelper.RunSQL(DBHelper.DBKind.DBApi, "UPDATE APP_USER_TRACE SET LOGOUT_TIME=dbo.FUNC_TO_DATE_TIME_INTEGER(getdate()),MEMBER_CODE=null WHERE APP_VERSION = @APP_VERSION and DEVICE_SERIES = @DEVICE_SERIES and PLATFORM=@PLATFORM AND LOGIN_TIME IS NOT NULL", parameters) > 0;

        }

        public void GetRoomDetail(String HotelID, String RoomNo)
        {
            RefreshRoomDetail detail = new RefreshRoomDetail(redisCacheManager, guestInfoService);
            detail.HotelID = HotelID;
            detail.RoomNo = RoomNo;
            detail.Run();

        }

        /// <summary>
        /// 从缓存中获取客房入住信息，如果缓存中没有则调用接口更新该数据
        /// </summary>
        /// <param name="HotelID">酒店ID</param>
        /// <param name="RoomNo">房间号</param>
        /// <returns>客房入住人员信息</returns>
        public List<Guest> GetRoomDetailFromCache(String HotelID, String RoomNo)
        {
            String key = "ROOMDETAIL:" + HotelID + ":" + RoomNo;
            String data = redisCacheManager.GetCache(key);
            if (data != null)
               return JsonConvert.DeserializeObject<List<Guest>>(data);

            var detail = new RefreshRoomDetail(redisCacheManager, guestInfoService);
            detail.HotelID = HotelID;
            detail.RoomNo = RoomNo;
            detail.Refresh();
            data = redisCacheManager.GetCache(key);
            return JsonConvert.DeserializeObject<List<Guest>>(data);
        }

        public string NextLevelHint(string memberID)
        {
            return guestInfoService.NextLevelHint(memberID);
        }

        public bool MobileIsMember(string mobile)
        {
            return guestInfoService.MobileIsMember(mobile);
        }

        #region 刷新会员信息
        private class RefreshRoomDetail
        {
            private IRedisCacheManager redisCacheManager;
            private IGetGuestInfoService guestInfoService;

            public String HotelID;
            public String RoomNo;

            public RefreshRoomDetail(IRedisCacheManager redisCacheManager, IGetGuestInfoService guestInfoService)
            {
                this.redisCacheManager = redisCacheManager;
                this.guestInfoService = guestInfoService;
            }

            public void Run()
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(Refresh)).Start();
            }

            public void Refresh()
            {
                var list = guestInfoService.GetInfoList(HotelID, RoomNo);
                String data = JsonConvert.SerializeObject(list);
                redisCacheManager.SetCache("ROOMDETAIL:" + HotelID + ":" + RoomNo, data);
            }

        }
        #endregion

    }
}
