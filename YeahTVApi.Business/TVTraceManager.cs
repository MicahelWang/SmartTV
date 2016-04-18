namespace HZTVApi.Manager
{
    using HZTVApi.Infrastructure;
    using HZTVApi.Common;
    using HZTVApi.Entity.NewEntity;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text;

    public class TVTraceManager : ITVTraceManager
    {
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string DEVICE_SERIES)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from TV_TRACE");
            strSql.Append(" where DEVICE_SERIES=@DEVICE_SERIES and APP_ID=@AppId");
            var parameters = new Dictionary<string, object>();
            parameters.Add("DEVICE_SERIES", DEVICE_SERIES); 
            return DBHelper.ScalarToInt(DBHelper.DBKind.DBApi, strSql.ToString(), parameters) > 0;
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(TV_TRACE_ALL model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into TV_TRACE(");
            strSql.Append("DEVICE_SERIES,APP_VERSION,FIRST_VISIT_TIME,LAST_VISIT_TIME,TV_KEY,IP,PLATFORM,BRAND,MANUFACTURER,MODEL,OS_VERSION,APP_ID,HOTEL_ID,ROOM_NO,MODEL_ID,ACTIVE,REMARK)");
            strSql.Append(" values (");
            strSql.Append("@DEVICE_SERIES,@APP_VERSION,@FIRST_VISIT_TIME,@LAST_VISIT_TIME,@TV_KEY,@IP,@PLATFORM,@BRAND,@MANUFACTURER,@MODEL,@OS_VERSION,@APP_ID,@HOTEL_ID,@ROOM_NO,@MODEL_ID,@ACTIVE,@REMARK)");
            SqlParameter[] parameters = {
					new SqlParameter("@DEVICE_SERIES", SqlDbType.NVarChar,128),
					new SqlParameter("@APP_VERSION", SqlDbType.NVarChar,64),
					new SqlParameter("@FIRST_VISIT_TIME", SqlDbType.BigInt,8),
					new SqlParameter("@LAST_VISIT_TIME", SqlDbType.BigInt,8),
					new SqlParameter("@TV_KEY", SqlDbType.NVarChar,64),
					new SqlParameter("@IP", SqlDbType.NVarChar,64),
					new SqlParameter("@PLATFORM", SqlDbType.NVarChar,64),
					new SqlParameter("@BRAND", SqlDbType.NVarChar,64),
					new SqlParameter("@MANUFACTURER", SqlDbType.NVarChar,64),
					new SqlParameter("@MODEL", SqlDbType.NVarChar,64),
					new SqlParameter("@OS_VERSION", SqlDbType.NVarChar,64),
					new SqlParameter("@APP_ID", SqlDbType.NVarChar,64),
					new SqlParameter("@HOTEL_ID", SqlDbType.NVarChar,7),
					new SqlParameter("@ROOM_NO", SqlDbType.NVarChar,7),
					new SqlParameter("@MODEL_ID", SqlDbType.Int,4),
					new SqlParameter("@ACTIVE", SqlDbType.Bit,1),
					new SqlParameter("@REMARK", SqlDbType.NVarChar,512)};
            parameters[0].Value = model.DEVICE_SERIES;
            parameters[1].Value = model.APP_VERSION;
            parameters[2].Value = model.FIRST_VISIT_TIME;
            parameters[3].Value = model.LAST_VISIT_TIME;
            parameters[4].Value = model.TV_KEY;
            parameters[5].Value = model.IP;
            parameters[6].Value = model.PLATFORM;
            parameters[7].Value = model.BRAND;
            parameters[8].Value = model.MANUFACTURER;
            parameters[9].Value = model.MODEL;
            parameters[10].Value = model.OS_VERSION;
            parameters[11].Value = model.APP_ID;
            parameters[12].Value = model.HOTEL_ID;
            parameters[13].Value = model.ROOM_NO;
            parameters[14].Value = model.MODEL_ID;
            parameters[15].Value = model.ACTIVE;
            parameters[16].Value = model.REMARK;
            int rows = DBHelper.RunSQL(DBHelper.DBKind.DBApi, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(TV_TRACE_ALL model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TV_TRACE set ");
            strSql.Append("APP_VERSION=@APP_VERSION,");
            strSql.Append("FIRST_VISIT_TIME=@FIRST_VISIT_TIME,");
            strSql.Append("LAST_VISIT_TIME=@LAST_VISIT_TIME,");
            strSql.Append("TV_KEY=@TV_KEY,");
            strSql.Append("IP=@IP,");
            strSql.Append("BRAND=@BRAND,");
            strSql.Append("MANUFACTURER=@MANUFACTURER,");
            strSql.Append("MODEL=@MODEL,");
            strSql.Append("OS_VERSION=@OS_VERSION,");
            strSql.Append("APP_ID=@APP_ID,");
            strSql.Append("HOTEL_ID=@HOTEL_ID,");
            strSql.Append("ROOM_NO=@ROOM_NO,");
            strSql.Append("MODEL_ID=@MODEL_ID,");
            strSql.Append("ACTIVE=@ACTIVE,");
            strSql.Append("REMARK=@REMARK");
            strSql.Append(" where DEVICE_SERIES=@DEVICE_SERIES and PLATFORM=@PLATFORM ");
            SqlParameter[] parameters = {
					new SqlParameter("@APP_VERSION", SqlDbType.NVarChar,64),
					new SqlParameter("@FIRST_VISIT_TIME", SqlDbType.BigInt,8),
					new SqlParameter("@LAST_VISIT_TIME", SqlDbType.BigInt,8),
					new SqlParameter("@TV_KEY", SqlDbType.NVarChar,64),
					new SqlParameter("@IP", SqlDbType.NVarChar,64),
					new SqlParameter("@BRAND", SqlDbType.NVarChar,64),
					new SqlParameter("@MANUFACTURER", SqlDbType.NVarChar,64),
					new SqlParameter("@MODEL", SqlDbType.NVarChar,64),
					new SqlParameter("@OS_VERSION", SqlDbType.NVarChar,64),
					new SqlParameter("@APP_ID", SqlDbType.NVarChar,64),
					new SqlParameter("@HOTEL_ID", SqlDbType.NVarChar,7),
					new SqlParameter("@ROOM_NO", SqlDbType.NVarChar,7),
					new SqlParameter("@MODEL_ID", SqlDbType.Int,4),
					new SqlParameter("@ACTIVE", SqlDbType.Bit,1),
					new SqlParameter("@REMARK", SqlDbType.NVarChar,512),
					new SqlParameter("@DEVICE_SERIES", SqlDbType.NVarChar,128),
					new SqlParameter("@PLATFORM", SqlDbType.NVarChar,64)};
            parameters[0].Value = model.APP_VERSION;
            parameters[1].Value = model.FIRST_VISIT_TIME;
            parameters[2].Value = model.LAST_VISIT_TIME;
            parameters[3].Value = model.TV_KEY;
            parameters[4].Value = model.IP;
            parameters[5].Value = model.BRAND;
            parameters[6].Value = model.MANUFACTURER;
            parameters[7].Value = model.MODEL;
            parameters[8].Value = model.OS_VERSION;
            parameters[9].Value = model.APP_ID;
            parameters[10].Value = model.HOTEL_ID;
            parameters[11].Value = model.ROOM_NO;
            parameters[12].Value = model.MODEL_ID;
            parameters[13].Value = model.ACTIVE;
            parameters[14].Value = model.REMARK;
            parameters[15].Value = model.DEVICE_SERIES;
            parameters[16].Value = model.PLATFORM;

            int rows = DBHelper.RunSQL(DBHelper.DBKind.DBApi, strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TV_TRACE_ALL GetModel(string DEVICE_SERIES)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 DEVICE_SERIES,APP_VERSION,FIRST_VISIT_TIME,LAST_VISIT_TIME,TV_KEY,IP,PLATFORM,BRAND,MANUFACTURER,MODEL,OS_VERSION,APP_ID,HOTEL_ID,ROOM_NO,MODEL_ID,ACTIVE,REMARK from TV_TRACE ");
            strSql.Append(" where DEVICE_SERIES=@DEVICE_SERIES and APP_ID=@AppId");
            var parameters = new Dictionary<string, object>();
            parameters.Add("DEVICE_SERIES", DEVICE_SERIES);
            TV_TRACE_ALL model = new TV_TRACE_ALL();
            DataTable dt = DBHelper.QueryToDataTable(DBHelper.DBKind.DBApi, strSql.ToString(), parameters);
            if (null != dt && dt.Rows.Count > 0)
            {
                return DataRowToModel(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<TV_TRACE_ALL> GetPageList(string hotelId, int pageIndex, int pageSize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  DEVICE_SERIES,APP_VERSION,FIRST_VISIT_TIME,LAST_VISIT_TIME,TV_KEY,IP,PLATFORM,BRAND,MANUFACTURER,MODEL,OS_VERSION,APP_ID,HOTEL_ID,ROOM_NO,MODEL_ID,ACTIVE,REMARK from TV_TRACE ");
            strSql.Append(" where HOTEL_ID=@HOTEL_ID ORDER BY ROOM_NO");
            var parameters = new Dictionary<string, object>();
            parameters.Add("HOTEL_ID", hotelId);
            var query = DBHelper.QueryToDataTable(DBHelper.DBKind.DBApi, strSql.ToString(), parameters);

            List<TV_TRACE_ALL> list = new List<TV_TRACE_ALL>();
            TV_TRACE_ALL trace = null;
            foreach (DataRow dataRow in query.Rows)
            {
                trace = DataRowToModel(dataRow);
                list.Add(trace);
            }
            var pagedList = new PagedList<TV_TRACE_ALL>(list, pageIndex, pageSize);
            return pagedList;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public TV_TRACE_ALL DataRowToModel(DataRow row)
        {
            TV_TRACE_ALL model = new TV_TRACE_ALL();
            if (row != null)
            {
                if (row["DEVICE_SERIES"] != null)
                {
                    model.DEVICE_SERIES = row["DEVICE_SERIES"].ToString();
                }
                if (row["APP_VERSION"] != null)
                {
                    model.APP_VERSION = row["APP_VERSION"].ToString();
                }
                if (row["FIRST_VISIT_TIME"] != null && row["FIRST_VISIT_TIME"].ToString() != "")
                {
                    model.FIRST_VISIT_TIME = long.Parse(row["FIRST_VISIT_TIME"].ToString());
                }
                if (row["LAST_VISIT_TIME"] != null && row["LAST_VISIT_TIME"].ToString() != "")
                {
                    model.LAST_VISIT_TIME = long.Parse(row["LAST_VISIT_TIME"].ToString());
                }
                if (row["TV_KEY"] != null)
                {
                    model.TV_KEY = row["TV_KEY"].ToString();
                }
                if (row["IP"] != null)
                {
                    model.IP = row["IP"].ToString();
                }
                if (row["PLATFORM"] != null)
                {
                    model.PLATFORM = row["PLATFORM"].ToString();
                }
                if (row["BRAND"] != null)
                {
                    model.BRAND = row["BRAND"].ToString();
                }
                if (row["MANUFACTURER"] != null)
                {
                    model.MANUFACTURER = row["MANUFACTURER"].ToString();
                }
                if (row["MODEL"] != null)
                {
                    model.MODEL = row["MODEL"].ToString();
                }
                if (row["OS_VERSION"] != null)
                {
                    model.OS_VERSION = row["OS_VERSION"].ToString();
                }
                if (row["APP_ID"] != null)
                {
                    model.APP_ID = row["APP_ID"].ToString();
                }
                if (row["HOTEL_ID"] != null)
                {
                    model.HOTEL_ID = row["HOTEL_ID"].ToString();
                }
                if (row["ROOM_NO"] != null)
                {
                    model.ROOM_NO = row["ROOM_NO"].ToString();
                }
                if (row["MODEL_ID"] != null && row["MODEL_ID"].ToString() != "")
                {
                    model.MODEL_ID = int.Parse(row["MODEL_ID"].ToString());
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
                if (row["REMARK"] != null)
                {
                    model.REMARK = row["REMARK"].ToString();
                }
            }
            return model;
        }
    }
}
