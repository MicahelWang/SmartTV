using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ConditionModels
{
    public class UserCondition
    {
       /// <summary>
       /// 集团编号
       /// </summary>
       [Display(Name = "集团")]
        public string UGroupId { get; set; }
        [Display(Name = "酒店")]
        public string UHotelId { get; set; }

        [Display(Name = "状态")]
        public int? UState { get; set; }
        [Display(Name = "用户类型")]
        public int? EUserType { get; set; }
        [Display(Name = "关键字")]
        public string KeyWord { get; set; }

    }
}
