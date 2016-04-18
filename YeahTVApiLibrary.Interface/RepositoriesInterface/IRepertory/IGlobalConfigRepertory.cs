using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DomainModels;

namespace YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory
{
    public interface IGlobalConfigRepertory : IBsaeRepertory<GlobalConfig>
    {
        List<GlobalConfig> SearchByHotelId(string hotelId, string brandId, string groupId);
        List<GlobalConfig> SearchByBrandId(string brandId, string groupId);
        List<GlobalConfig> SearchByGroupId(string groupId);
        GlobalConfig GetGlobalConfig(GlobalConfig globalConfig);
      
    }
}
