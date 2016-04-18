using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    /// <summary>
    /// android应用程序信息
    /// </summary>
    public class AndroidInfo
    {
        public string Name { get; set; }

        public List<AndroidSetting> Settings { get; set; }
    }

    /// <summary>
    /// 设置
    /// </summary>
    public class AndroidSetting
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class AndroidInfos
    {
        public List<AndroidInfo> Infos { get; set; }

        public string FilePath { get; set; }
    }
}
