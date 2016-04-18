using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

using NBearLite;
using System.Data;
using System.Data.Common;
using NBearLite.Emit;

namespace YeahTVApi.Manager
{
    public class DBHelper
    {
        public class Parameter
        {
            public string name;
            public DbType dbType;
            public int size;
            public ParameterDirection direction;
            public object value;

            public static Parameter CreateOut(DbType dbType, int size)
            {
                return CreateOut(string.Empty, dbType, size);
            }

            public static Parameter CreateOut(string name, DbType dbType, int size)
            {
                return new Parameter
                {
                    direction = ParameterDirection.Output,
                    name = name,
                    dbType = dbType,
                    size = size
                };
            }
        }

        public enum DBKind
        {
            DBApi
        };

        private static NBearLite.Database GetDataBase(DBKind db)
        {
            string strDB = "HTApi";

            if (db == DBKind.DBApi)
            {
                strDB = "HTApi";
            }
            else
            {
                strDB = "HTApi";
            }
            Database rst = new Database(strDB);
            return rst;
        }
        private static void SetParameters(NBearLite.Database ds, DbCommand cmd, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (KeyValuePair<string, object> kv in parameters)
                {
                    if (kv.Value is Parameter)
                    {
                        Parameter p = kv.Value as Parameter;
                        if (p.direction == ParameterDirection.Input)
                        {
                            ds.AddInParameter(cmd, kv.Key, p.value);
                        }
                        else if (p.direction == ParameterDirection.Output)
                        {
                            ds.AddOutParameter(cmd, kv.Key, p.dbType, p.size);
                        }
                    }
                    else
                    {
                        ds.AddInParameter(cmd, kv.Key, kv.Value);
                    }
                }

                ds.AddParameter(cmd, "ReturnValue", DbType.Int32, ParameterDirection.ReturnValue, null,
                    DataRowVersion.Default, null);
            }
        }

        private static int GetParameters(NBearLite.Database ds, DbCommand cmd, Dictionary<string, object> parameters)
        {
            int returnValue = 0;
            foreach (DbParameter pam in cmd.Parameters)
            {
                if (pam.Direction == ParameterDirection.InputOutput ||
                    pam.Direction == ParameterDirection.Output ||
                    pam.Direction == ParameterDirection.ReturnValue)
                {
                    string key = pam.ParameterName;
                    bool find = false;
                    if (parameters.ContainsKey(key))
                    {
                        find = true;
                    }

                    if (find)
                    {
                        object obj = parameters[key];
                        if (obj is Parameter)
                        {
                            Parameter p = obj as Parameter;
                            if (p != null) p.value = pam.Value;
                        }
                    }
                    else if (pam.Direction == ParameterDirection.ReturnValue)
                    {
                        returnValue = (int)pam.Value;
                    }
                }
            }
            return returnValue;
        }

        public static int RunSQL(DBKind db, string sql, Dictionary<string, object> parameters)
        {
            Database ds = GetDataBase(db);
            DbCommand cmd = ds.GetSqlStringCommand(sql);
            SetParameters(ds, cmd, parameters);
            return ds.ExecuteNonQuery(cmd);
        }
       
        public static DataSet ExecuteDataSet(DBKind db, string sp, Dictionary<string, object> parameters,out int returnValue)
        {
            Database ds = GetDataBase(db);
            DbCommand cmd = ds.GetStoredProcCommand(sp);
            SetParameters(ds, cmd, parameters);
            DataSet dst = ds.ExecuteDataSet(cmd);
            returnValue = GetParameters(ds, cmd, parameters);
            return dst;
        }
        public static DataTable ExecuteDataTable(DBKind db, string sp, Dictionary<string, object> parameters)
        {
            int returnValue = 0;
            DataSet ds = ExecuteDataSet(db, sp, parameters, out returnValue);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            if (dt == null)
            {
                throw new YeahTVApi.Entity.ApiException("数据表错误");
            }
            return dt;
        }

        public static DataTable ExecuteDataTable(DBKind db, string sp, Dictionary<string, object> parameters,out int returnValue)
        {
            DataSet ds = ExecuteDataSet(db, sp, parameters, out returnValue);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            if (dt == null)
            {
                throw new YeahTVApi.Entity.ApiException("数据表错误");
            }
            return dt;
        }


        public static DataRow ExecuteDataRow(DBKind db, string sp, Dictionary<string, object> parameters)
        {
            DataTable dt = ExecuteDataTable(db, sp, parameters);
            if (dt!= null &&　dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public static DataRow ExecuteDataRow(DBKind db, string sp, Dictionary<string, object> parameters,out int returnValue)
        {
            DataTable dt = ExecuteDataTable(db, sp, parameters, out returnValue);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        public static DataSet QueryToDataSet(DBKind db, string sql, Dictionary<string, object> parameters)
        {
            Database ds = GetDataBase(db);
            DbCommand cmd = ds.GetSqlStringCommand(sql);
            SetParameters(ds, cmd, parameters);
            return ds.ExecuteDataSet(cmd);
        }

        public static IDataReader QueryToReader(DBKind db, string sql, Dictionary<string, object> parameters)
        {
            Database ds = GetDataBase(db);
            DbCommand cmd = ds.GetSqlStringCommand(sql);
            SetParameters(ds, cmd, parameters);
            return ds.ExecuteReader(cmd);
           
        }

        public static IDataReader QueryToReader(DBKind db, string sql, Dictionary<string, object> parameters,CommandType type)
        {
            Database ds = GetDataBase(db);
            DbCommand cmd = ds.GetSqlStringCommand(sql);
            SetParameters(ds, cmd, parameters);
            cmd.CommandType = type;
            return ds.ExecuteReader(cmd);

        }

        public static DataTable QueryToDataTable(DBKind db, string sql, Dictionary<string, object> parameters)
        {
            DataSet ds = QueryToDataSet(db, sql, parameters);
            DataTable dt = null;
            if (ds != null && ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            if (dt == null)
            {
                throw new YeahTVApi.Entity.ApiException("数据表错误");
            }
            return dt;
        }

        public static DataRow QueryToDataRow(DBKind db, string sql, Dictionary<string, object> parameters)
        {
            DataTable dt = QueryToDataTable(db, sql, parameters);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0];
            }
            else
            {
                return null;
            }
        }

        #region ExecuteScalar
        public static object ExecScalar(DBKind db, string sp, Dictionary<string, object> parameters)
        {
            object rst;

            Database ds = GetDataBase(db);
            DbCommand cmd = ds.GetStoredProcCommand(sp);
            SetParameters(ds, cmd, parameters);
            rst = ds.ExecuteScalar(cmd);

            if (Convert.IsDBNull(rst)) rst = null;

            return rst;
        }

        public static object QueryScalar(DBKind db, string sql, Dictionary<string, object> parameters)
        {
            object rst;

            Database ds = GetDataBase(db);
            DbCommand cmd = ds.GetSqlStringCommand(sql);
            SetParameters(ds, cmd, parameters);
            rst = ds.ExecuteScalar(cmd);

            if (Convert.IsDBNull(rst)) rst = null;

            return rst;
        }

        public static string ScalarToString(DBKind db, string sql, Dictionary<string, object> parameters)
        {
            return ScalarToString(db, sql, parameters, string.Empty);
        }

        public static string ScalarToString(DBKind db, string sql, Dictionary<string, object> parameters, string def)
        {
            string rst = def;
            object obj = QueryScalar(db, sql, parameters);
            if (obj != null)
            {
                rst = Convert.ToString(obj);
            }
            return rst;
        }

        public static int ScalarToInt(DBKind db, string sql, Dictionary<string, object> parameters, int def = 0)
        {
            int rst = def;
            object obj = QueryScalar(db, sql, parameters);
            if (obj != null)
            {
                rst = Convert.ToInt32(obj);
            }
            return rst;
        }





        public static int RunSQL(DBKind db, string sql, SqlParameter[] parameters)
        {
            Database ds = GetDataBase(db);
            DbCommand cmd = ds.GetSqlStringCommand(sql);
            cmd.Parameters.AddRange(parameters);
            int flag = ds.ExecuteNonQuery(cmd);
            cmd.Parameters.Clear();
            return flag;
        }

        
        #endregion
    }
}
