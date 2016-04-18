using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum HCSJobOperationType
    {
        [Description("上架")]
        Shelve = 1,
        [Description("下架")]
        UnShelve = 2
    }
}