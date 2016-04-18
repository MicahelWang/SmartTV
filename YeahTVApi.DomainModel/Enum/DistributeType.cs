using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum DistributeType
    {
        [Description("分发全部(剩余酒店)")]
        All,
        [Description("取消已分发")]
        Cancel,
        [Description("分发部分")]
        Part
    }
}
