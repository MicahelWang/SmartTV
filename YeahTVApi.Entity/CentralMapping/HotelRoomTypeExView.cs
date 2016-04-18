using System;
using System.Runtime.Serialization;

namespace YeahTVApi.Entity.CentralMapping
{
    [Serializable]    
    public class HotelRoomTypeExView
    {
        public int ID { get; set; }
        public string HotelID { get; set; }
        public string RoomTypeID { get; set; }
       
        public string BedType { get; set; }
       
        public string BedWidth { get; set; }
       
        public string HotelArea { get; set; }
       
        public string FloorPlace { get; set; }
       
        public string IsHasWindow { get; set; }
       
        public string HasSomkeFree { get; set; }
       
        public string IsADSL { get; set; }
       
        public string IsADSLFee { get; set; }
       
        public string IsADSLWifi { get; set; }
       
        public string MaxCheckInPeoNum { get; set; }
       
        public string ImageUrl { get; set; }
    }
}
