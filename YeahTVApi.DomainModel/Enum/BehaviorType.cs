using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum BehaviorType
    {
        [Description("模块使用时长")]
        ModuleUsed,
        [Description("电影点播")]
        MovieVod,
        [Description("TV观看时长")]
        ChannelUsed,
        [Description("其他")]
        Other,
        [Description("HCS错误信息")]
        HCSERROR,
        [Description("HCS调试信息")]
        HCSDEBUG
    }
}
