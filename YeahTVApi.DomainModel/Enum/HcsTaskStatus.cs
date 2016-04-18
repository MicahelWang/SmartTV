using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum HcsTaskStatus
    {
        [Description("分发")]
        Normal = 0,
        [Description("取消分发")]
        Cancel = 1
    }
}