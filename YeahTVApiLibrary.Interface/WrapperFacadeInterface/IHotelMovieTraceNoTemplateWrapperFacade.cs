using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IHotelMovieTraceNoTemplateWrapperFacade
    { 
        List<HotelMovieTraceNoTemplate> GetHotelMovieTraceWithLocalizeList(List<HotelMovieTraceNoTemplate> list);
        List<HotelMovieTraceNoTemplate> SearchHotelMovieTraceNoTemplates(HotelMovieTraceNoTemplateCriteria criteria);
        void AddHotelMovieTraceNoTemplates(List<HotelMovieTraceNoTemplate> templates);
        void AddHotelMovieTraceNoTemplate(HotelMovieTraceNoTemplate template);
        void Update(HotelMovieTraceNoTemplate template);
        int Update(Expression<Func<HotelMovieTraceNoTemplate, bool>> Predicate, Expression<Func<HotelMovieTraceNoTemplate, HotelMovieTraceNoTemplate>> Updater);
        void Delete(HotelMovieTraceNoTemplate template);

        void Distribute(DistributeType distributeType, MovieForLocalize movie, List<CoreSysHotel> allHotels, string lastUpdateUser,
            ICollection<string> existHotels = null);

        void DistributeByHotel(CoreSysHotel hotel);

        void DistributeByDevice(DeviceTrace device);

        [Cache]
        List<HotelMovieTraceNoTemplate> SearchHotelMovieTraceNoTemplatesFromCache(HotelMovieTraceNoTemplateCriteria criteria);
    }
}
