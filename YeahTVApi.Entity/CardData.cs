using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 写卡的数据
    /// </summary>
    public class CardData
    {
        public String HotelName;
        public String HotelID;
        public String RoomNO;
        /// <summary>
        /// Base64格式
        /// </summary>
        public String Data;
        /// <summary>
        /// 块号
        /// </summary>
        public int Block;

        /// <summary>
        /// 扇区号
        /// </summary>
        public int Sector;
        /// <summary>
        /// 写卡密码
        /// </summary>
        public String WritePassword;

        [JsonIgnore]
        public byte[] byteData;

        public String EndDate;
    }
}
