using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum BindingType
    {
        [Description("全部酒店")]
        ALL,
        [Description("单个酒店")]
        Hotel
    }
}
