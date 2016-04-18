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

    public class OrderQRCodeRecordRepertory : BaseRepertory<OrderQRCodeRecord, string>, IOrderQRCodeRecordRepertory
    {
        public override List<OrderQRCodeRecord> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as OrderQRCodeRecordCriteria;

            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(criteria.OrderId))
                query = query.Where(q => q.OrderId.Equals(criteria.OrderId));

            if (!string.IsNullOrEmpty(criteria.OrderType))
                query = query.Where(q => q.OrderType.Contains(criteria.OrderType));

            if (!string.IsNullOrEmpty(criteria.Ticket))
                query = query.Where(q => q.Ticket.Contains(criteria.Ticket));

            return query.ToPageList(criteria);
        }
    }
}
