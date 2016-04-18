using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity.CentralMapping
{

    #region Result
    /// <summary>
    /// 查询商业配套设施详情结果
    /// </summary>
    public class QueryBusinessDetailResult : OperationResult
    {
        /// <summary>
        /// 商业配套设施详情对象集合
        /// PS:当前使用的是大众点评提供的数据,详见wiki数据字典
        /// </summary>
        public List<BusinessDetail> BusinessDetailList { get; set; }
    }
    #endregion
}
