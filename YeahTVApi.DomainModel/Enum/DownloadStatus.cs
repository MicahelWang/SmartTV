using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum DownloadStatus
    {
        [Description("未执行")]
        NotSynchronize = 0,
        [Description("同步失败")]
        Fail = -1,
        [Description("同步成功")]
        Success = 1
    }
}