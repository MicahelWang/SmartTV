using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface ITVHotelConfigManager : IBaseManager<TVHotelConfig, HotelConfigCriteria>
    {
        void AddTVHotelConfig(TVHotelConfig tVHotelConfig);

        void UpdateTVHotelConfig(TVHotelConfig tVHotelConfig);

        TVHotelConfig GetEntity(int id);

        TVHotelConfig GetHotelConfig(HotelConfigCriteria criteria);
        List<TVHotelConfig> SearhTVHotelConfig(HotelConfigCriteria criteria);

        void AddTVHotelConfig(List<TVHotelConfig> tVHotelConfigs);
        List<string> SearchOnlyHotelId(BaseSearchCriteria searchCriteria);
        void AddHotelPaymentConfig(string hotelId);
        int Update(Expression<Func<TVHotelConfig, bool>> Predicate, Expression<Func<TVHotelConfig, TVHotelConfig>> Updater);
    }
}
