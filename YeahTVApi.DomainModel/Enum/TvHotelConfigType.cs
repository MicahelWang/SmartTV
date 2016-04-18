using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Enum
{
    public enum TvHotelConfigType
    {
        [Description("Vod点播地址")]
        VodAddress,
        [Description("电视服务器地址")]
        ChannleAddress,
        [Description("应用服务器IP地址")]
        AppAddress,
        [Description("酒店PMS地址")]
        PMSAddress
    }
}
