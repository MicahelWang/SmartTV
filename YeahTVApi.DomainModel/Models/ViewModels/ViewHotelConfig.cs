using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApi.DomainModel.Models.ViewModels
{

    [Serializable]
    public class ViewHotelConfig
    {
        public CoreSysHotel CoreSysHotel { get; set; }
        public List<ViewHotelConfigItem> ViewHotelConfigItems { get; set; }
        public ViewHotelApp ViewHotelApps { get; set; }
    }

    [Serializable]
    public class ViewHotelConfigItem
    {
        public string ConfigName { get; set; }
        public string HotelId { get; set; }
        public int ItemsCount { get; set; }
        public int ExpectedItemsCount { get; set; }
        public string EditUrl { get; set; }
        public List<string> RightTips { get; set; }
        public List<string> WrongTips { get; set; }
    }

    [Serializable]
    public class ViewHotelApp
    {
        public List<AppPublish> Launcher { get; set; }
        public List<AppPublish> ThirdPartyApps { get; set; }
    }
}
