
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;

namespace YeahTVApi.Entity.CentralMapping
{
  

    #region Result

    /// <summary>
    /// 查询酒店结果对象
    /// </summary>
    public class QueryHotelResult : OperationResult
    {
        public QueryHotelResult()
        {

        }

        /// <summary>
        /// 查询得到的HotelModel集合
        /// </summary>
       
        public List<HotelModel> Data { get; set; }


        public List<HotelViewModel> HotelList
        {
            get;
            set;
        }



        /// <summary>
        /// 汉庭酒店总数
        /// </summary>
        public int HKCount
        {
            get;
            set;
        }

        /// <summary>
        /// 全季总数
        /// </summary>
        public int HTCount
        {
            get;
            set;
        }

        /// <summary>
        /// 客栈总数
        /// </summary>
        public int HZCount
        {
            get;
            set;
        }

        /// <summary>
        /// 星程总数
        /// </summary>
        public int XCCount
        {
            get;
            set;
        }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCout { get; set; }
    }

    #endregion

    #region ViewModel
    public class HotelViewModel
    {
        public HotelInfoViewModel Info { get; set; }

        
        public int Distance { get; set; }

        public List<RoomViewModel> Rooms { get; set; }

        public String Navigate360 { get; set; }
    }

    public class HotelInfoViewModel
    {
        /// <summary>
        /// 酒店房型照片
        /// </summary>
        public IEnumerable<HotelImage> HotelImage { get; set; }

        public List<string> HallIds;
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }

        public Boolean isOverBooked { get; set; }

        public Boolean IsUsualHotel { get; set; }
        /// <summary>
        /// 酒店最低价
        /// </summary>
        public decimal? HotelLowestPrice { get; set; }

        /// <summary>
        /// 酒店通知集合
        /// </summary>
        public IEnumerable<HotelNotice> HotelNotice { get; set; }

        /// <summary>
        /// 酒店设施
        /// </summary>
        public IEnumerable<HotelFacilityViewModel> HotelFacility { get; set; }

        /// <summary>
        /// 酒店照片
        /// </summary>
        public IEnumerable<HotelPhoto> HotelPhoto { get; set; }

        /// <summary>
        /// 酒店支持的信用卡
        /// </summary>
        public IEnumerable<HotelSupportCreditCardView> HotelSupportCreditCard { get; set; }

        /// <summary>
        /// 酒店商圈
        /// </summary>
        //public IEnumerable<HotelArea> HotelArea { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 酒店名称
        /// </summary>
        public string HotelName { get; set; }
        /// <summary>
        /// 酒店英文名称
        /// </summary>
        public string HotelNameEn { get; set; }
        /// <summary>
        /// 酒店品牌
        /// </summary>
        public HotelStyle HotelStyle { get; set; }
        /// <summary>
        /// 所属国家位置
        /// </summary>
        public string Nation { get; set; }
        /// <summary>
        /// 邮编
        /// </summary>
        public string Zip { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 酒店描述
        /// </summary>
        public string Descript { get; set; }


        /// <summary>
        /// 所在城市编号
        /// </summary>
        public int CityID { get; set; }
        /// <summary>
        /// 归属管辖分公司ID,从SSC库中同步;SSC表中的AreaID字段,在这里统称分公司ID
        /// </summary>
        //public int CompanyID { get; set; }
        /// <summary>
        /// 总房间数量
        /// </summary>
        public int RoomCount { get; set; }
        /// <summary>
        /// 经营类型 直营/加盟/市场联盟等
        /// </summary>
        public int HotelType { get; set; }
        /// <summary>
        /// 是否开业
        /// </summary>
        public bool IsOpening { get; set; }
        /// <summary>
        /// 营业日期
        /// </summary>
        public DateTime? BusinessDate { get; set; }
        /// <summary>
        /// 是否筹备中
        /// </summary>
        public bool IsShowPrepare { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public decimal CommentScore { get; set; }
        /// <summary>
        /// 评论数量
        /// </summary>
        public int CommentCount { get; set; }
        /// <summary>
        /// 酒店照片
        /// </summary>
        public string HeaderPhoto { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public decimal Lng { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public decimal Lat { get; set; }
        /// <summary>
        /// 是否仅限接待内宾
        /// </summary>
        public bool IsOnlyChinese { get; set; }
        /// <summary>
        /// 是否支持储值卡
        /// </summary>
        public bool IsSupportValueCardPay { get; set; }
        /// <summary>
        /// 是否开放预定
        /// </summary>
        public bool IsOpenResv { get; set; }
        /// <summary>
        /// 是否开放在线预定
        /// </summary>
        public bool IsOpenOnlineCheckIn { get; set; }
    }

    public class HotelFacilityViewModel
    {
        /// <summary>
        /// 服务设施ID
        /// </summary>
        public int FacilityID { get; set; }

        /// <summary>
        /// 服务设施名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 服务设施描述
        /// </summary>
        public string Descript { get; set; }

        /// <summary>
        /// 服务设施排序
        /// </summary>
        public int SortValue { get; set; }

        /// <summary>
        /// 服务设施类型ID
        /// </summary>
        public int FacilityTypeID { get; set; }

        /// <summary>
        /// 服务设施类型名称
        /// </summary>
        public string FacilityTypeName { get; set; }
    }

    public class RoomViewModel
    {
        /// <summary>
        /// 某个房型的门市价都是一致的. 应该放在房间对象中
        /// </summary>
        public string MarketPrice { get; set; }

        /// <summary>
        /// 房型ID
        /// </summary>
        
        public string RoomTypeID { get; set; }

        /// <summary>
        /// 房型名称
        /// </summary>
        
        public string RoomTypeName { get; set; }

        /// <summary>
        /// 成员级别
        /// </summary>
        
        //public string MemberLevel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        
        public RoomTypeModel Info { get; set; }
        /// <summary>
        /// 积分兑换免房所需积分
        /// </summary>
        
        public int ExchangePoint { get; set; }

        public List<RoomDetailViewModel> Details { get; set; }
        /// <summary>
        /// 最便宜的
        /// </summary>        
        public RoomDetailViewModel Cheapest { get; set; }
    }

    public class RoomDetailViewModel
    {
        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static explicit operator RoomDetail(RoomDetailViewModel model)
        {
            if (model == null)
                return null;
            RoomDetail detail = new RoomDetail();
            detail.ActivityID = model.ActivityID;
            detail.Name = model.Name;
            detail.IsOverBooked = model.IsOverBooked;
            detail.RoomType = model.RoomType;
            detail.MemberLevel = model.MemberLevel;
            detail.ActivityUrl = model.ActivityUrl;
            detail.Description = model.Description;
            detail.IconUrl = model.IconUrl;
            detail.Price = model.Price;
            if (model.BreakfastCount == 1)
                detail.BreakfastCount = "单早";
            else if (model.BreakfastCount == 0)
                detail.BreakfastCount = "";
            else if (model.BreakfastCount == 2)
                detail.BreakfastCount = "双早";
            else
                detail.BreakfastCount = model.BreakfastCount + "早";
            detail.IsBlocked = model.IsBlocked;
            detail.ShowResv = model.ShowResv;
            detail.MinStockCount = model.MinStockCount;
            detail.LessThan = model.LessThan;
            detail.IsMustOnlinePay = model.IsMustOnlinePay;
            return detail;
        }


        public Boolean IsBindMemberPrice { get; set; }


        public Boolean IsMustOnlinePay { get; set; }
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
        
        public int BreakfastCount { get; set; }

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

        // <summary>
        // 每日房量
        // </summary>
        //public List<RoomStockEntity> DailyRoomStock { get; set; }
    }

    #endregion
}