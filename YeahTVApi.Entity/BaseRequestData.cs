using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    public class BaseRequestData
    {
        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 应用ID
        /// </summary>
        public string APP_ID { get; set; }


        /// <summary>
        /// 用户Token，用户的标识通过该Token完成
        /// </summary>
        public string TOKEN { get; set; }

        /// <summary>
        /// 客户端语言
        /// </summary>
        public string language { get; set; }

        /// <summary>
        /// 设备序列号
        /// </summary>
        public string devNo { get; set; }

        /// <summary>
        /// 软件版本号
        /// </summary>
        public string ver { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 操作系统平台
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// 操作系统版本
        /// </summary>
        public string OSVersion { get; set; }
        /// <summary>
        /// 设备型号
        /// </summary>
        public string Model { get; set; }


        /// <summary>
        /// 纬度
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public string Longitude { get; set; }
    }
}
