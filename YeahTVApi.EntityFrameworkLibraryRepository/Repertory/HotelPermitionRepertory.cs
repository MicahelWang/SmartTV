using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApiLibrary.EntityFrameworkRepository;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class HotelPermitionRepertory : BaseRepertory<HotelPermition, string>, IHotelPermitionRepertory
    {
        public override List<HotelPermition> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HotelPermitionCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id == criteria.Id);

            if (!string.IsNullOrEmpty(criteria.UserId))
                query = query.Where(q => q.UserId.Equals(criteria.UserId));

            if (!string.IsNullOrEmpty(criteria.PermitionType))
                query = query.Where(q => q.PermitionType.Equals(criteria.PermitionType));

            if (!string.IsNullOrEmpty(criteria.TypeId))
                query = query.Where(q => q.TypeId.Equals(criteria.TypeId));

            return query.ToPageList(searchCriteria);
        }
    }
}
