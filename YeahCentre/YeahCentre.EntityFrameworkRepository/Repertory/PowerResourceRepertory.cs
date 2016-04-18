
using System.Collections.Generic;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class PowerResourceRepertory : BaseRepertory<ErpPowerResource, int>, IPowerResourceRepertory
    {
        public override List<ErpPowerResource> Search(BaseSearchCriteria searchCriteria)
        {
            throw new System.NotImplementedException();
        }
    }
}