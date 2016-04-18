namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;

    public interface ITVTraceRepertory : IDeviceTraceLibraryRepertory, IBsaeRepertory<DeviceTrace>
    {
        List<string> GetTraceHotelIds();
    }
}
