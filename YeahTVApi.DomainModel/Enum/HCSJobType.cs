using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum HCSJobType
    {
        [Description("电影")]
        VOD,
        [Description("应用")]
        APP,
        [Description("资源")]
        SRC
    }
}