namespace YeahTVApi.DomainModel.Mapping
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;

    public static class  MapTVTrace
    {
        public static DeviceTrace ToTVTrace(this RequestHeader header)
        {
            var trace = new DeviceTrace();

            trace.DeviceSeries = header.DEVNO;
            trace.Model = header.Model;
            trace.OsVersion = header.OSVersion;
            trace.Platfrom = header.Platform;
            trace.Brand = header.Brand;
            trace.Manufacturer = header.Manufacturer;
            trace.Ip = header.IP;
            trace.HotelId = header.HotelID;

            return trace;
        }
    }
}
