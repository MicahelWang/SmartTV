//using HZ.ReservationGateway.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 
    /// </summary>
    public class GetRecommendActivityListResult
    {
        #region Result
        /// <summary>
        /// 
        /// </summary>
        internal IList<ActivityEntity> Data { get; set; }

        /// <summary>
        /// 酒店交通视图集合
        /// </summary>
        public List<ActivityRecommendViewModel> ActivityRecommendList
        {
            get;
            set;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class ActivityRecommendViewModel
    {
        /// <summary>
        /// 市场活动编号
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 市场活动名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否支持使用促销券
        /// </summary>
        public bool IsSetPromotionCode { get; set; }
    }


    public static partial class ViewModelExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pActivityRecommend"></param>
        /// <returns></returns>
        public static ActivityRecommendViewModel ToViewModel(this ActivityEntity pActivityRecommend)
        {
            return new ActivityRecommendViewModel
            {
                ID = pActivityRecommend.ActivityID,
                Name = pActivityRecommend.ActivityName,
                IsSetPromotionCode = pActivityRecommend.IsSetPromotionCode,
            };
        }

        #endregion
    }

}
