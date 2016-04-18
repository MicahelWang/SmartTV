using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class Template01
    {

        /// <summary>
        /// 字体库地址
        /// </summary>
        public string FontUrl { get; set; }
        /// <summary>
        /// 选中模块背景
        /// </summary>
        public string ModuleOnBackgrand { get; set; }
        /// <summary>
        /// 未选中模块背景
        /// </summary>
        public string ModuleOffBackgrand { get; set; }
        /// <summary>
        /// 模块选中背景
        /// </summary>
        public string ModuleSelectedBackgrand { get; set; }

        public List<ModuleFor01> Modules { get; set; }

        /// <summary>
        /// 背景
        /// </summary>
        public string Backgrand { get; set; }

        public string WelcomeWorld { get; set; }

        public string HotelLogo { get; set; }

        public string Tip { get; set; }
    }

    public class ModuleFor01
    {
        /// <summary>
        /// 跳转类型{1 app 2 html}
        /// </summary>
        public int IntentType { get; set; }
        /// <summary>
        /// 跳转路径
        /// </summary>
        public string IntentPath { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }
        public string DisplayNameEn { get; set; }
        /// <summary>
        /// 选中图片
        /// </summary>
        public string OnIcon { get; set; }

        public string OffIcon { get; set; }
        /// <summary>
        /// 模块类型
        /// {1:首页，2:多屏互动 3:语言 0:default}
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 展示图片
        /// </summary>
        public string ModuleImg { get; set; }
    }
}
