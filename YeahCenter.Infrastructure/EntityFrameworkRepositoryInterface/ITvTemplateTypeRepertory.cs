using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApi.Infrastructure.RepositoriesInterface.EntityFrameworkRepositoryInterface.IRepertory.YeahCentre
{
    public interface ITvTemplateTypeRepertory : IBsaeRepertory<TvTemplateType>
    {
        IPagedList<TvTemplateType> PagedList(int pageIndex, int pageSize, string keyword);
    }
}
