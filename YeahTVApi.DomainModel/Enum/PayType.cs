using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum PayType
    {
        [Description("单部点播")]
        Movie = 0,
        [Description("包天")]
        Daily = 1
    }
}