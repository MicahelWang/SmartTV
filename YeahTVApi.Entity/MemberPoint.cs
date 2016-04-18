using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 会员积分信息汇总
    /// </summary>
    public class MemberPointInfo
    {
        public int currentPoint { get; set; }
        public List<MemberPoint> detail { get; set; }
        public int pageCount;
        public int recordCount;
    }

    /// <summary>
    /// 会员积分信息明细
    /// </summary>
    public class MemberPoint
    {
        public string logDate { get; set; }
        public string remark { get; set; }
        public string m1 { get; set; }
        public string credit { get; set; }
        public string charge { get; set; }
        public string descript { get; set; }
        public string expiryDate { get; set; }
        public string balance { get; set; }
    }
}
