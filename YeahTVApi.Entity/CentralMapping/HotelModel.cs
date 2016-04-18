using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class HotelModel
    {
    

        /// <summary>
        /// 酒店对象挂接的酒店实体对象
        /// </summary>
        
        public HotelView Info { get; set; }

        /// <summary>
        /// 酒店相对于查询经纬度的偏差值
        /// </summary>
        public int distance { get; set; }

        /// <summary>
        /// 酒店经纬度偏差值
        /// </summary>

        public int Distance
        {
            get;
            set;
        }

        /// <summary>
        /// 市场活动列表
        /// </summary>
        internal List<ActivityEntity> ActivityList { get; set; }

     
        /// <summary>
        /// 酒店相关房间对象集合属性
        /// </summary>

        public List<RoomModel> Rooms
        {
            get;
            set;
        }

     

    
       
    }
}
