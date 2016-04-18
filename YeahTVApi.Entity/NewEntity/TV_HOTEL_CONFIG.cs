using System;
namespace YeahTVApi.Entity.Model
{
	/// <summary>
	/// TV_HOTEL_CONFIG:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TV_HOTEL_CONFIG
	{
		public TV_HOTEL_CONFIG()
		{}
		#region Model
		private int _id;
		private string _hotel_id;
		private string _config_code;
		private string _config_name;
		private string _config_value;
		private bool _active;
		private string _last_updater;
		private long? _last_update_time;
		private long? _create_time;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HOTEL_ID
		{
			set{ _hotel_id=value;}
			get{return _hotel_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CONFIG_CODE
		{
			set{ _config_code=value;}
			get{return _config_code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CONFIG_NAME
		{
			set{ _config_name=value;}
			get{return _config_name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CONFIG_VALUE
		{
			set{ _config_value=value;}
			get{return _config_value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool ACTIVE
		{
			set{ _active=value;}
			get{return _active;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LAST_UPDATER
		{
			set{ _last_updater=value;}
			get{return _last_updater;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long? LAST_UPDATE_TIME
		{
			set{ _last_update_time=value;}
			get{return _last_update_time;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long? CREATE_TIME
		{
			set{ _create_time=value;}
			get{return _create_time;}
		}
		#endregion Model

	}
}

