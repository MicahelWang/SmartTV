namespace YeahTVApiLibrary.Infrastructure
{
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;

    public interface IVODPaymentRequestManager
    {
        void Add(VODPaymentRequest vODPaymentRequest);
    }
}
