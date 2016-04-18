using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using HZTVApi.Common;
using HZTVApi.Entity;
using HZ.Web.Authorization;
using HZTVApi.Infrastructure;

namespace HZTVApi.Business
{
    public class ApiDBManager : IApiDBManager
    {
        //用于和 sha(userpass + key) 之后用于密码加密
        private const string key = "shjgad%^$!@^16524^%@!^@$^%&$!219726761827351C&^!@%$&%!2416524";
        private IRedisCacheManager redisCacheManager;

        public ApiDBManager( IRedisCacheManager redisCacheManager)
        {
            this.redisCacheManager = redisCacheManager;
        }

        public ApiDBManager()
        {
            this.redisCacheManager = new RedisCacheManager();
        }

        /// <summary>
        /// 查询酒店房型照片
        /// </summary>
        /// <param name="HotelID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsAllow(string Mobile)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@Mobile", Mobile);
            try
            {
                int value = (int)DBHelper.QueryScalar(DBHelper.DBKind.DBApi, "SELECT count(1) FROM TEMP_USER WHERE Mobile=@Mobile", parameters);
                return value > 0;
            }
            catch (Exception err)
            {
                HTOutputLog.SaveError("IsAllow", err);


            }
            return true;
        }

        public DataRow GetMemberInfoEx(string vno)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@vno", vno);

            DataRow rst = DBHelper.ExecuteDataRow(DBHelper.DBKind.DBApi, "apisp1_QueryMemberInfoEx", parameters);
            return rst;
        }

        public bool ValidCode(string mobile, string ValidCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("MOBILE", mobile);
            parameters.Add("VALIDCODE", ValidCode);
            parameters.Add("EXPIRE_TIME", DateTime.Now.ToString("yyyyMMddHHmmss"));

            object count = DBHelper.QueryScalar(DBHelper.DBKind.DBApi, "SELECT COUNT(1) FROM [APP_MOBILE_VALIDCODE] with(nolock) WHERE MOBILE=@MOBILE AND VALIDCODE=@VALIDCODE and EXPIRE_TIME>=@EXPIRE_TIME", parameters);
            return !count.Equals(0);
        }

        public void DeleteValidCode(String mobile)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("MOBILE", mobile);
            DBHelper.QueryScalar(DBHelper.DBKind.DBApi, "DELETE [APP_MOBILE_VALIDCODE] WHERE MOBILE=@MOBILE", parameters);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="devNo"></param>
        /// <returns></returns>
        public void SaveMobileCheckNo(String mobile, String ValidCode)
        {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("MOBILE", mobile);
            parameters.Add("VALIDCODE", ValidCode);
            parameters.Add("EXPIRE_TIME", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));

            object count = DBHelper.QueryScalar(DBHelper.DBKind.DBApi, "SELECT COUNT(1) FROM [APP_MOBILE_VALIDCODE] with(nolock) WHERE MOBILE=@MOBILE", parameters);

            if (0.Equals(count))
            {
                DBHelper.RunSQL(DBHelper.DBKind.DBApi, @"INSERT INTO [APP_MOBILE_VALIDCODE]
                    VALUES(@MOBILE,@VALIDCODE,@EXPIRE_TIME)", parameters);
            }
            else
            {
                DBHelper.RunSQL(DBHelper.DBKind.DBApi, @"UPDATE [APP_MOBILE_VALIDCODE]
                    SET VALIDCODE=@VALIDCODE, EXPIRE_TIME=@EXPIRE_TIME Where MOBILE=@MOBILE", parameters);
            }

        }
  
        public String GetHotelRoomKey(RequestHeader header)
        {
            //登录信息写入数据库
            var parameters = new Dictionary<string, object>();
            parameters.Add("APP_VERSION", header.Ver);
            parameters.Add("DEVICE_SERIES", header.DEVNO);
            object obj =
            DBHelper.QueryScalar(DBHelper.DBKind.DBApi, @"SELECT TV_KEY FROM [TV_TRACE] WITH (NOLOCK) WHERE
            APP_VERSION=@APP_VERSION AND DEVICE_SERIES=@DEVICE_SERIES", parameters);
            if (obj != null)
                return obj.ToString();
            return null;

        }

        /// <summary>
        /// 刷新该店的这些数据
        /// </summary>
        /// <param name="header"></param>
        public void RefreshDeviceHotel(RequestHeader header)
        {
            //登录信息写入数据库
            var parameters = new Dictionary<string, object>();
            parameters.Add("APP_VERSION", header.Ver); 
            parameters.Add("DEVICE_SERIES", header.DEVNO); 
            var row =
            DBHelper.QueryToDataRow(DBHelper.DBKind.DBApi, @"SELECT HOTEL_ID,ROOM_NO FROM [TV_TRACE] WITH (NOLOCK) WHERE
            APP_VERSION=@APP_VERSION  AND DEVICE_SERIES=@DEVICE_SERIES", parameters);
            if (row != null)
            {
                header.hotelID = row[0].ToString();
                header.roomNo = row[1].ToString();
            }

            redisCacheManager.SetCache(header.DEVNO, header.hotelID + ";" + header.roomNo);
        }

        public String GetMemberID(String Token)
        {
            //登录信息写入数据库
            var parameters = new Dictionary<string, object>();
            parameters.Add("TOKEN", Token);
            object obj =
            DBHelper.QueryScalar(DBHelper.DBKind.DBApi, @"SELECT MEMBER_CODE FROM [APP_USER_TRACE] WHERE TOKEN=@TOKEN AND 
            EXPIRE_TIME>=dbo.FUNC_TO_DATE_TIME_INTEGER(getdate())", parameters);
            if (obj != null)
                return obj.ToString();
            return null;

        }

        public String GetSignKey(String Token)
        {
            //登录信息写入数据库
            var parameters = new Dictionary<string, object>();
            parameters.Add("TOKEN", Token);

            object obj =
            DBHelper.QueryScalar(DBHelper.DBKind.DBApi,
            String.Format(@"SELECT {0} FROM [APP_USER_TRACE] WHERE TOKEN=@TOKEN AND EXPIRE_TIME>=dbo.FUNC_TO_DATE_TIME_INTEGER(getdate())", ResponseParameter.SignKey), parameters);
            if (obj != null)
                return obj.ToString();
            return null;

        }

        public void UpdateTokenExpireTime(String Token, out String resultKey)
        {

            //登录信息写入数据库
            var parameters = new Dictionary<string, object>();
            parameters.Add("TOKEN", Token);

            if (DBHelper.RunSQL(DBHelper.DBKind.DBApi, @"UPDATE [APP_USER_TRACE]
            SET EXPIRE_TIME=dbo.FUNC_TO_DATE_TIME_INTEGER(getdate()+7)     
            WHERE TOKEN=@TOKEN", parameters) > 0)
            {
                resultKey = DBHelper.QueryScalar(DBHelper.DBKind.DBApi, "SELECT GUEST_KEY1 from [APP_USER_TRACE] WITH (noLOCK) WHERE TOKEN=@TOKEN", parameters).ToString();
            }
            else
            {

                throw new Exception("非法用户资料");
            }

        }
 
        public Boolean SaveDeviceHotelRelation(String devNo, String hotelid, String remark)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("SERIES", devNo);
            parameters.Add("HOTELID", hotelid);
            parameters.Add("REMARK", remark);
            int obj = DBHelper.RunSQL(DBHelper.DBKind.DBApi, @"
INSERT INTO [DEVICE_HOTEL_RELATION]
           ([DEVICE_SERIES]
           ,[HOTEL_CODE]
           ,[ACTIVE]
           ,[LAST_UPDATER]
           ,[LAST_UPDATE_TIME]
           ,[CREATE_TIME]
           ,[TAG]
           ,[REMARK])
     VALUES
           (@SERIES
           ,@HOTELID
           ,1
           ,'WEB'
           ,dbo.FUNC_TO_DATE_TIME_INTEGER(getdate())
           ,dbo.FUNC_TO_DATE_TIME_INTEGER(getdate())
           ,NULL
           ,@REMARK)", parameters);
            return obj == 1;
        }

        /// <summary>
        /// 是否发放过优惠券
        /// </summary>
        /// <param name="MemberCode">会员号</param>
        /// <returns>true 发放过, false 没有发放过</returns>
        public Boolean IsGivedCoupon(String MemberCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("MEMBER_CODE", MemberCode);
            var value = DBHelper.QueryScalar(DBHelper.DBKind.DBApi, "SELECT ID FROM APP_FIRSTLOGIN WHERE MEMBER_CODE=@MEMBER_CODE", parameters);
            return !(value == DBNull.Value || value == null);

        }

        public Boolean SetGivedCoupon(String MemberCode)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("MEMBER_CODE", MemberCode);
            var value = DBHelper.RunSQL(DBHelper.DBKind.DBApi, "INSERT INTO APP_FIRSTLOGIN VALUES(@MEMBER_CODE,'','1',dbo.FUNC_TO_DATE_TIME_INTEGER(getdate()))", parameters);
            return value == 1;

        }

        /// <summary>
        /// 记录用户访问数据
        /// </summary>
        /// <param name="DEVICE_SERIES"></param>
        /// <param name="APP_VERSION"></param>
        /// <param name="PLATFORM"></param>
        /// <param name="MEMBER_CODE"></param>
        /// <param name="ACCESS_MODE"></param>
        /// <param name="IP"></param>
        /// <param name="LATITUDE"></param>
        /// <param name="LONGITUDE"></param>
        /// <param name="CHANNEL"></param>
        /// <param name="guestkey"></param>
        /// <returns></returns>
        public DataRow LogUserTrace(BaseRequestData data, String MEMBER_CODE)
        {
            //判断一下有没有该用户访问的TOKEN，如果有则获取该Token，否则生成一个新的Token


            //登录信息写入数据库
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("CHANNEL", data.APP_ID);
            parameters.Add("DEVICE_SERIES", data.devNo);
            parameters.Add("APP_VERSION", data.ver);
            parameters.Add("PLATFORM", data.Platform);
            parameters.Add("ACCESS_MODE", null);
            parameters.Add("LATITUDE", data.Latitude);
            parameters.Add("LONGITUDE", data.Longitude);
            parameters.Add("IP", null);
            if (string.IsNullOrEmpty(MEMBER_CODE))
            {
                parameters.Add("MEMBER_CODE", "");
                parameters.Add("GUEST_KEY1", "");
                parameters.Add("GUEST_KEY2", "");
                parameters.Add("LOGIN_TIME", null);
            }
            else
            {

                parameters.Add("MEMBER_CODE", MEMBER_CODE);
                parameters.Add("LOGIN_TIME", DateTime.Now.ToString("yyyyMMddHHmmss"));
                string gkey = Convert.ToBase64String(SecurityManager.GetHash(SecurityManager.HashType.SHA1, data.TOKEN + key + MEMBER_CODE));
                parameters.Add("GUEST_KEY1", gkey);
                parameters.Add("GUEST_KEY2", data.TOKEN);

            }
            return DBHelper.ExecuteDataRow(DBHelper.DBKind.DBApi, "[SP_APP_USER_TRACE]", parameters);
        }

        /// <summary>
        ///  记录设备访问数据
        /// </summary>
        /// <param name="dict">字典数据</param>
        /// <param name="status">返回状态，用来标识是否成功</param>
        /// <returns></returns>
        public DataSet LogDeviceTrace(RequestHeader header, out int status, out String tv_key)
        {
            //登录信息写入数据库
            var parameters = new Dictionary<string, object>();
            parameters.Add("DEVICE_SERIES", header.DEVNO);
            parameters.Add("MODEL", header.Model);
            parameters.Add("OS_VERSION", header.OSVersion);
            parameters.Add("APP_VERSION", header.Ver);
            parameters.Add("PLATFORM", header.Platform);
            parameters.Add("APP_ID", header.APP_ID);
            parameters.Add("BRAND", header.Brand);
            parameters.Add("MANUFACTURER", header.Manufacturer);
            parameters.Add("IP", header.IP);
            tv_key=SecurityManager.GetRNGString(30, 2);
            parameters.Add("DEVICE_KEY", tv_key);
            var row = DBHelper.ExecuteDataSet(DBHelper.DBKind.DBApi, "[SP_TV_TRACE]", parameters, out status);
            return row;
        }


        /// <summary>
        /// 返回应用程序列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetTVTrace(RequestHeader header)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("DEVICE_SERIES", header.DEVNO);
            //登录信息写入数据库
            return DBHelper.QueryToReader(DBHelper.DBKind.DBApi, @"SELECT *
  FROM [TV_TRACE] with (nolock) WHERE  DEVICE_SERIES=@DEVICE_SERIES", parameters);

        }

        /// <summary>
        /// 返回应用程序列表
        /// </summary>
        /// <returns></returns>
        public IDataReader GetAppVersionList()
        {
            //登录信息写入数据库
            return DBHelper.QueryToReader(DBHelper.DBKind.DBApi, @"SELECT [ID]
      ,[NAME]
      ,[PLATFORM]
      ,[DESCRIPTION]
      ,[APP_KEY]
      ,[ACTIVE]
      ,[SECURE_KEY]
  FROM [TV_APPS] WHERE Active=1", null);

        }

        /// <summary>
        /// 获取酒店ID
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public string GetHotelID(RequestHeader header)
        {
            if (header.hotelID == null)
            {
                RefreshDeviceHotel(header);
            }
            return header.hotelID;
        }

        /// <summary>
        /// 获取酒店ID
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public string GetRoomNo(RequestHeader header)
        {
            if (header.roomNo == null)
            {
                RefreshDeviceHotel(header);
            }
            return header.roomNo;
        }

    }

}
