using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YeahTVApi.Entity.CentralMapping;

namespace YeahTVApi.Entity
{
    public class RoomDetail
    {



        /// <summary>
        /// 价格名称，如标准价，XXX活动价
        /// </summary>

        public string Name { get; set; }
        /// <summary>
        /// 市场活动ID，标准价时此值为空
        /// </summary>

        public string ActivityID { get; set; }
        /// <summary>
        /// 是否满房
        /// todo:
        /// </summary>

        public bool IsOverBooked { get; set; }
        /// <summary>
        /// 房间类型
        /// </summary>

        public string RoomType { get; set; }
        /// <summary>
        /// 酒店编号, 便于前台绑定
        /// </summary>
        /// <summary>
        /// 会员级别
        /// </summary>
        public string MemberLevel { get; set; }
        /// <summary>
        /// 市场活动链接
        /// </summary>       
        public string ActivityUrl { get; set; }
        /// <summary>
        /// 描述
        /// </summary>       
        public string Description { get; set; }
        /// <summary>
        /// 图标链接地址
        /// </summary>

        public string IconUrl { get; set; }
        ///// <summary>
        ///// 
        ///// </summary>
        //
        //public string BannerUrl { get; set; }
        /// <summary>
        /// 第一日房价
        /// </summary>

        public string Price { get; set; }
        /// <summary>
        /// 第一日早餐份数
        /// </summary>
        public String BreakfastCount { get; set; }

        /// <summary>
        /// 房型不可预订
        /// todo:
        /// </summary>

        public bool IsBlocked { get; set; }

        /// <summary>
        /// 从DailyRoomStock集合中进行判断, 如果所有房间数量>0, 则为true
        /// </summary>

        public bool ShowResv { get; set; }

        /// <summary>
        /// 从DailyRoomStock集合中进行判断, 如果所有房间数量<5, 则为true
        /// </summary>
        public bool LessThan { get; set; }


        public int MinStockCount { get; set; }

        public Boolean IsMustOnlinePay { get; set; }
    }
}
