using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface ITVHotelConfigRepertory : IBsaeRepertory<TVHotelConfig>
    {
        List<string> SearchOnlyHotelId(BaseSearchCriteria searchCriteria);
    }
}