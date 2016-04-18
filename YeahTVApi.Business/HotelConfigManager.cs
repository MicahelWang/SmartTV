namespace HZTVApi.Manager
{
    using HZTVApi.Entity.Model;
    using HZTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class HotelConfigManager : IHotelConfigManager
    { 
        /// <summary>
        /// 获取酒店配置项通过配置code
        /// </summary>
        /// <param name="HotelId"></param>
        /// <param name="Config_Code"></param>
        /// <returns></returns>
        public TV_HOTEL_CONFIG GetTvHotelConfigByCode(String HotelId, String Config_Code)
        {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("HOTEL_ID", HotelId);
            parameters.Add("CONFIG_CODE", Config_Code);
            DataRow dr = DBHelper.QueryToDataRow(DBHelper.DBKind.DBApi, "SELECT * FROM TV_HOTEL_CONFIG WHERE ACTIVE=1 AND HOTEL_ID=@HOTEL_ID AND CONFIG_CODE=@CONFIG_CODE ", parameters);
            if (null != dr)
            {
                return DataRowToModel(dr);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取酒店配置项所有配置项
        /// </summary>
        /// <param name="HotelId"></param>
        /// <param name="Config_Code"></param>
        /// <returns></returns>
        public List<TV_HOTEL_CONFIG> GetTvHotelConfig(String HotelId)
        {
            List<TV_HOTEL_CONFIG> list = null;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("HOTEL_ID", HotelId);
            DataTable dt = DBHelper.QueryToDataTable(DBHelper.DBKind.DBApi, "SELECT * FROM TV_HOTEL_CONFIG WHERE ACTIVE=1 AND HOTEL_ID=@HOTEL_ID ", parameters);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                list.Add(DataRowToModel((dt.Rows[i])));
            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        private TV_HOTEL_CONFIG DataRowToModel(DataRow row)
        {
            TV_HOTEL_CONFIG model = new TV_HOTEL_CONFIG();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["HOTEL_ID"] != null)
                {
                    model.HOTEL_ID = row["HOTEL_ID"].ToString();
                }
                if (row["CONFIG_CODE"] != null)
                {
                    model.CONFIG_CODE = row["CONFIG_CODE"].ToString();
                }
                if (row["CONFIG_NAME"] != null)
                {
                    model.CONFIG_NAME = row["CONFIG_NAME"].ToString();
                }
                if (row["CONFIG_VALUE"] != null)
                {
                    model.CONFIG_VALUE = row["CONFIG_VALUE"].ToString();
                }
                if (row["ACTIVE"] != null && row["ACTIVE"].ToString() != "")
                {
                    if ((row["ACTIVE"].ToString() == "1") || (row["ACTIVE"].ToString().ToLower() == "true"))
                    {
                        model.ACTIVE = true;
                    }
                    else
                    {
                        model.ACTIVE = false;
                    }
                }
                if (row["LAST_UPDATER"] != null)
                {
                    model.LAST_UPDATER = row["LAST_UPDATER"].ToString();
                }
                if (row["LAST_UPDATE_TIME"] != null && row["LAST_UPDATE_TIME"].ToString() != "")
                {
                    model.LAST_UPDATE_TIME = long.Parse(row["LAST_UPDATE_TIME"].ToString());
                }
                if (row["CREATE_TIME"] != null && row["CREATE_TIME"].ToString() != "")
                {
                    model.CREATE_TIME = long.Parse(row["CREATE_TIME"].ToString());
                }
            }
            return model;
        }
    }
}
