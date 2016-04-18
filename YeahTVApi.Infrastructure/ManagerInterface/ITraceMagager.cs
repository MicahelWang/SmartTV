namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;

    public interface ITraceManager : IDeviceTraceLibraryManager
    {       
        /// <summary>
        /// 查找设备通过设备号
        /// </summary>
        /// <param name="header">DEVNO</param>
        /// <returns></returns>
        DeviceTrace GetDevice(RequestHeader header);
    }
}
