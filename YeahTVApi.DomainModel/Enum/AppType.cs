using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum AppType
    {
        [Description("智能电视")]
        TV,
        [Description("应用中心")]
        AppCenter,
        [Description("后台系统")]
        CommonFramework,
        [Description("酒店核心系统")]
        HCS,
        [Description("YeahCenterApi接口")]
        YeahCenterApi,
    }
}
