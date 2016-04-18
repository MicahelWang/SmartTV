using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    public class HotelStyleResult : OperationResult
    {
        /// <summary>
        /// 酒店品牌视图集合
        /// </summary>
        public List<HotelStyleView> HotelStyleList { get; set; }
    }
}
