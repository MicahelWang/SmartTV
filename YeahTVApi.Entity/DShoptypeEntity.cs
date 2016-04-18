using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace YeahTVApi.Entity
{
    /// <summary>
    /// Summary description for DShoptype
    /// </summary>
    public class DShoptypeEntity
    {
        /// <summary>
        /// 频道图标
        /// </summary>
        public string CategoryIcon { set; get; }
        /// <summary>
        /// 频道ID
        /// </summary>
        public string CategoryID { set; get; }
        /// <summary>
        /// 频道名
        /// </summary>
        public string CategoryName { set; get; }
        /// <summary>
        /// 所属上一级分类的ID（此接口返回值统一为0）
        /// </summary>
        public int ParentID { set; get; }
        /// <summary>
        /// 所属频道ID（此接口返回值与CategoryID一致）
        /// </summary>
        public int ShopType { set; get; }
    }
}