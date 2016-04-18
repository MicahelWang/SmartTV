using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 酒店其他信息类型枚举
    /// </summary>
    [Flags]
    public enum HotelOtherInfoType
    {
        None        = 0x0000,
        Remark      = 0x0001,
        Price       = 0x0002,
        Activity    = 0x0004,
        Service     = 0x0008,
        CardType    = 0x0010,
        UsualHotel  = 0x0020,
        HotelImage  = 0x0040,
        Promotion   = 0x0080,
        AreaName    = 0x0100,

        List        = 0x0100,
        All         = 0xFFFF
    }
    /// <summary>
    /// 查询酒店接口
    /// </summary>
    public interface IQueryHotelManager
    {
        List<Hotel> QueryList();
        List<Hotel> QueryListByHotelIDS(string ids);
        HotelOtherInfoType OtherInfo { get; set; }
        
        /// <summary>
        /// 当前登录用户信息
        /// </summary>
        object MemberInfo { get; set; }

        #region 属性
        string ActivityCode { get; set; }
        string Area { get; set; }
        DateTime CheckInDate { get; set; }
        DateTime CheckOutDate { get; set; }
        string CityID { get; set; }
        string Distence { get; set; }
        string ECoupon { get; set; }
        string GEO { get; set; }
        string HotelID { get; set; }
        string HotelName { get; set; }
        string HotelStyle { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int PageRecordCount { get; set; }
        string SortBy { get; set; }
    
        #endregion
    }
}
