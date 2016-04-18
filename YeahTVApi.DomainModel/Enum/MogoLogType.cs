using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum LogType
    {
        [Description("信息")]
        Infomation,
        [Description("警告")]
        Waring,
        [Description("错误")]
        Error,
        [Description("用户行为")]
        UserBehavior
    }
}
