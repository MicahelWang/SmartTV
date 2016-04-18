using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum DeviceType
    {
        [Description("一体机")]
        AIO,
        [Description("机顶盒")]
        STB,
        [Description("HCS服务器")]
        HCSServer,

    }
}
