namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel;
    using EntityFramework.Extensions;

    public class VODPaymentRequestRepertory : BaseRepertory<VODPaymentRequest, string>, IVODPaymentRequestRepertory
    {
        public override List<VODPaymentRequest> Search(BaseSearchCriteria searchCriteria)
        {
            return null;
        }
    }
}
