using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class ViewCoreSysHotel
    {
        [DisplayName("APM位置")]
        public string AMPLocation { get; set; }
        [DisplayName("APL位置")]
        public string APLocation { get; set; }
         [DisplayName("机房位置")]
        public string ComputerRoomLocation { get; set; }
        public string Connected { get; set; }
         [DisplayName("联系人")]
        public string Contact { get; set; }
        public string ELVLocation { get; set; }
        public string EngineeringMenu { get; set; }
         [DisplayName("楼层数")]
        public string  FloorNumber { get; set; }
         [DisplayName("网络状态")]
        public string NETStatus { get; set; }
        [DisplayName("其他")]
        public string Other { get; set; }
        public string PlugType { get; set; }
        public string RoomNumberDetail { get; set; }
         [DisplayName("房间号")]
        public string RoomNumber { get; set; }
        [DisplayName("服务器位置")]
        public string ServerLocation { get; set; }
        [DisplayName("TV品牌")]
        public string TVBrand { get; set; }
         [DisplayName("TV位置")]
        public string TVLocation { get; set; }
        [DisplayName("Wifi状态")]
        public string WIFIStatus { get; set; }
        public List<string> imagelists { get; set; }
    }
}
