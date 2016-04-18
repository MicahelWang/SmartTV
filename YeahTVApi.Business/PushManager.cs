namespace HZTVApi.Manager
{
    using HZTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class PushManager : IPushManager
    {

        /// <summary>
        /// 注册推送绑定关系
        /// </summary>
        /// <param name="CHANNEL_ID">推送渠道ID</param>
        /// <param name="PUSH_TOKEN">推送Token</param>
        /// <param name="DEVICE_SERIES">设备序列号</param>
        /// <param name="PLATFORM">平台</param>
        /// <returns></returns>
        public bool PushRegister(String CHANNEL_ID, string PUSH_TOKEN, String DEVICE_SERIES,String PLATFORM,String APP_VERSION)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("CHANNEL_ID", CHANNEL_ID);
            parameters.Add("PUSH_TOKEN", PUSH_TOKEN);
            parameters.Add("PLATFORM", PLATFORM);
            parameters.Add("DEVICE_SERIES", DEVICE_SERIES);
            parameters.Add("APP_VERSION", APP_VERSION);
            //更新数据
            String SQL = "UPDATE APP_USER_TRACE SET PUSH_TOKEN=@PUSH_TOKEN,CHANNEL_ID=@CHANNEL_ID WHERE APP_VERSION = @APP_VERSION and DEVICE_SERIES = @DEVICE_SERIES and PLATFORM=@PLATFORM";
            return DBHelper.RunSQL(DBHelper.DBKind.DBApi, SQL, parameters) > 0;


        }

        /// <summary>
        /// 注册推送绑定关系
        /// </summary>
        /// <param name="CHANNEL_ID">推送渠道ID</param>
        /// <param name="PUSH_TOKEN">推送Token</param>
        /// <param name="DEVICE_SERIES">设备序列号</param>
        /// <param name="PLATFORM">平台</param>
        /// <returns></returns>
        public bool PushRegister(String CHANNEL_ID, string PUSH_TOKEN, String DEVICE_SERIES, String PLATFORM, String APP_VERSION, String Member_ID)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("CHANNEL_ID", CHANNEL_ID);
            parameters.Add("PUSH_TOKEN", PUSH_TOKEN);
            parameters.Add("PLATFORM", PLATFORM);
            parameters.Add("DEVICE_SERIES", DEVICE_SERIES);
            parameters.Add("MEMBER_ID", Member_ID);
            //更新数据
            String SQL = "app_push.dbo.SP_Push_REGISTER";
            return DBHelper.ExecScalar(DBHelper.DBKind.DBApi, SQL, parameters) != null;


        }
    }
}
