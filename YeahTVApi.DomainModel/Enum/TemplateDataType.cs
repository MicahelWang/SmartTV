using System.ComponentModel;

namespace YeahTVApi.DomainModel.Enum
{
    public enum TemplateDataType
    {
        [Description("文本")] Text = 0,
        [Description("图片")] Image = 1,
        [Description("音频")] Audio = 2,
        [Description("视频")] Video = 3,
        [Description("列表")] List = 4,
        [Description("颜色")] Color = 5
    }
}