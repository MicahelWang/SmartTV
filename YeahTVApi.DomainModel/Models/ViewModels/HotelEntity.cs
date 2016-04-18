using System;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class HotelEntity
    {

        public string HotelId { get; set; }
        public string HotelCode { get; set; }
        public string HotelName { get; set; }
        public string HotelNameEn { get; set; }

        public string GroupId { get; set; }

        public string BrandId { get; set; }
        public bool IsLocalPms { get; set; }
        public string Tel { get; set; }

        public int Province { get; set; }

        public int City { get; set; }
        public int Country { get; set; }
        public string Address { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public string TemplateId { get; set; }
        public bool AutoToHome { get; set; }
        public int Languages { get; set; }
        public string WelcomeWord { get; set; }
        public string LaunchBackground { get; set; }
        public string LocalPmsUrl { get; set; }
        public decimal PriceOfDay { get; set; }
        public string AdUrl { get; set; }

        public string BaseData { get; set; }
        public string LogoImageUrl { get; set; }
        public string VodAddress { get; set; }

        public string DeviceInfo { get; set; }
        public string HoteDeviceInfo { get; set; }
    }

    public class HotelRoomEntity
    {
        public string RoomNo { get; set; }
        public string HotelId { get; set; }
        public string HotelCode { get; set; }
        public string HotelName { get; set; }
        public string HotelNameEn { get; set; }

        public string GroupId { get; set; }

        public string BrandId { get; set; }
        public bool IsLocalPms { get; set; }
        public string Tel { get; set; }

        public int Province { get; set; }

        public int City { get; set; }
        public int Country { get; set; }
        public string Address { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public string TemplateId { get; set; }
        public bool AutoToHome { get; set; }
        public int Languages { get; set; }
        public string WelcomeWord { get; set; }
        public string LaunchBackground { get; set; }
        public string LocalPmsUrl { get; set; }
        public decimal PriceOfDay { get; set; }
        public string AdUrl { get; set; }

        public string BaseData { get; set; }
        public string LogoImageUrl { get; set; }
        public string VodAddress { get; set; }

        public string DeviceInfo { get; set; }
        public string HoteDeviceInfo { get; set; }
    }

    public class SimpleHotelEntity
    {
        public string HotelId { get; set; }
        public string HotelCode { get; set; }
        public string HotelName { get; set; }
        public string Address { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public string Logo { get; set; }
    }

    [Obsolete]
    public class ObsoleteSimpleHotel
    {
        public string HotelName { get; set; }
        public string HotelNameEn { get; set; }
        public bool IsLocalPms { get; set; }
        public string Tel { get; set; }

        public string TemplateId { get; set; }
        public bool AutoToHome { get; set; }
        public int Languages { get; set; }
        public string WelcomeWord { get; set; }
        public string LaunchBackground { get; set; }
        public string LocalPmsUrl { get; set; }
        public decimal PriceOfDay { get; set; }
        public string AdUrl { get; set; }
    }
    public class SimpleHotel
    {

        public string HotelName { get; set; }
        public string HotelNameEn { get; set; }
        public bool IsLocalPms { get; set; }
        public int Languages { get; set; }
        public string LogoImageUrl { get; set; }
        public string LocalPmsUrl { get; set; }
    }
}