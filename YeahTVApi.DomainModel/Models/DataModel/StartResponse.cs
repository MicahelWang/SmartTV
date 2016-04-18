using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class StartResponse 
    {

        /// <summary>
        /// 应用的配置信息
        /// </summary>
        public Dictionary<String, String> ConfigData { get; set; }

        /// <summary>
        /// 数据加密密钥，用该密钥对请求的数据进行加密
        /// </summary>
        public string SecureKey { get; set; }

       // public SimpleHotel Hotel { get; set; }
        public Object Hotel { get; set; }

        public StartResource StartResource { get; set; }

        public object TemplateContent { get; set; }

        public string RoomNo { get; set; }
    }

    public class StartResource
    {
        public string FloatImageUrl { get; set; }
        public string LogoImageUrl { get; set; }
        public string BackgroundImageUrl { get; set; }
        public string BackgroundMusicUrl { get; set; }
    }
}
