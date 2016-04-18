using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{

    #region Result
    /// <summary>
    /// 酒店交通路线查询结果集
    /// </summary>
    public class GetHotelRouteInfoListResult : OperationResult
    {
        /// <summary>
        /// 
        /// </summary>
        internal List<HotelRoute> Data { get; set; }

        /// <summary>
        /// 酒店交通视图集合
        /// </summary>
        public List<HotelRoute> HotelRouteList
        {
            get;
            set;
        }
    }

    public class HotelRouteViewModel
    {
        /// <summary>
        /// 服务信息集合
        /// </summary>
        public string HotelID { get; set; }
        /// <summary>
        /// 目的地
        /// </summary>
        public string RouteFrom { get; set; }
        /// <summary>
        /// 具体描述
        /// </summary>
        public string Description { get; set; }
    }

    #endregion

   
}
