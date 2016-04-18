using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 查看优惠券详情结果类
    /// </summary>
    public class QueryProjectRulesResult:OperationResult
    {
     
        public IList<string> ProjectRules { get; set; }
    }
}
