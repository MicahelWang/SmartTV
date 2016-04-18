using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    /// <summary>
    ///图片轮播配置项
    /// </summary>
    public class HomeConfigModel
    {
        public HomeConfigModel()
        { }

        public List<string> home_pictures { get; set; }
        public int home_pictures_cycle_time { get; set; }
        public string home_background_musaudio { get; set; }
    }
}
