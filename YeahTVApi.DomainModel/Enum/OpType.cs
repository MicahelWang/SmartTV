using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum OpType
    {
        [Description("查看")]
        View = 1,
        [Description("新增")]
        Add = 2,
        [Description("更新")]
        Update = 3,
        [Description("复制")]
        Copy = 4,
        [Description("删除")]
        Delete = 5,
    }
}