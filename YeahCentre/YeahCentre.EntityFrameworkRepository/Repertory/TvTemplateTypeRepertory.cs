using System.Collections.Generic;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Infrastructure.RepositoriesInterface.EntityFrameworkRepositoryInterface.IRepertory.YeahCentre;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class TvTemplateTypeRepertory : BaseRepertory<TvTemplateType, int>, ITvTemplateTypeRepertory
    {
        public override List<TvTemplateType> Search(BaseSearchCriteria searchCriteria)
        {
            throw new System.NotImplementedException();
        }

        public IPagedList<TvTemplateType> PagedList(int pageIndex, int pageSize, string keyword)
        {
            var query = this.Entities.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(m => m.Name.Contains(keyword));

            query = query.OrderBy(m => m.Id);
            return new PagedList<TvTemplateType>(query, pageIndex, pageSize);
        }
    }
}
