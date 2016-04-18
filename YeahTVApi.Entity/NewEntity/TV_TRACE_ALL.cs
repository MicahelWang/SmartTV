using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.NewEntity
{
    /// <summary>
    /// TV_TRACE:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class TV_TRACE_ALL
    {
        public TV_TRACE_ALL()
        { }
        #region Model
        private string _device_series;
        private string _app_version;
        private long _first_visit_time;
        private long _last_visit_time;
        private string _tv_key;
        private string _ip;
        private string _platform;
        private string _brand;
        private string _manufacturer;
        private string _model;
        private string _os_version;
        private string _app_id;
        private string _hotel_id;
        private string _room_no;
        private int _model_id;
        private bool _active;
        private string _remark;
        /// <summary>
        /// 
        /// </summary>
        public string DEVICE_SERIES
        {
            set { _device_series = value; }
            get { return _device_series; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APP_VERSION
        {
            set { _app_version = value; }
            get { return _app_version; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long FIRST_VISIT_TIME
        {
            set { _first_visit_time = value; }
            get { return _first_visit_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public long LAST_VISIT_TIME
        {
            set { _last_visit_time = value; }
            get { return _last_visit_time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TV_KEY
        {
            set { _tv_key = value; }
            get { return _tv_key; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PLATFORM
        {
            set { _platform = value; }
            get { return _platform; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BRAND
        {
            set { _brand = value; }
            get { return _brand; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MANUFACTURER
        {
            set { _manufacturer = value; }
            get { return _manufacturer; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MODEL
        {
            set { _model = value; }
            get { return _model; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OS_VERSION
        {
            set { _os_version = value; }
            get { return _os_version; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string APP_ID
        {
            set { _app_id = value; }
            get { return _app_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string HOTEL_ID
        {
            set { _hotel_id = value; }
            get { return _hotel_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ROOM_NO
        {
            set { _room_no = value; }
            get { return _room_no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int MODEL_ID
        {
            set { _model_id = value; }
            get { return _model_id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool ACTIVE
        {
            set { _active = value; }
            get { return _active; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string REMARK
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model

    }
}
