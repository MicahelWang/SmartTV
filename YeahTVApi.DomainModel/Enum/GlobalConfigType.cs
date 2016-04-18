using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum GlobalConfigType
    {
        [Description("集团")]
        Group,
        [Description("品牌")]
        Brand,
        [Description("酒店")]
        Hotel
    }
}
