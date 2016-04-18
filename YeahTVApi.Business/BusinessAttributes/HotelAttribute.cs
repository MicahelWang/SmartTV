using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HZTVApi.Entity;
using HZTVApi.Entity.CentralMapping;
using HZTVApi.Common;
using HZ.Web.Authorization;

namespace HZTVApi.Business.BusinessAttributes
{
    /// <summary>
    /// 酒店详情转换类
    /// </summary>
    public class HotelAttribute : BusinessAttribute
    {

        /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public HotelAttribute()
            : base(typeof(CentralApiResult<List<HotelInfo>>))
        {

        }


        public CentralApiResult<List<HotelInfo>> GetHotelList()
        {
            
            return null;

        }

    }
}
