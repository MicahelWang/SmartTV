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
    using System;

    public class VODOrderRepertory : BaseRepertory<VODOrder, string>, IVODOrderRepertory
    {
        public override List<VODOrder> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as VODOrderCriteria;

            var query = base.Entities.AsQueryable().Where(m => !m.IsDelete);

            if (!string.IsNullOrEmpty(criteria.OrderId))
                query = query.Where(q => q.Id.Equals(criteria.OrderId));

            if (!string.IsNullOrEmpty(criteria.MovieId))
                query = query.Where(q => q.MovieId.Equals(criteria.MovieId));

            if (!string.IsNullOrEmpty(criteria.SeriseCode))
                query = query.Where(q => q.SeriseCode.Equals(criteria.SeriseCode));

            if (!string.IsNullOrEmpty(criteria.RoomNo))
                query = query.Where(q => q.RoomNo.Equals(criteria.RoomNo));

            if (criteria.orderState.HasValue)
                query = query.Where(q => q.State.Equals((int)criteria.orderState.Value));

            if (criteria.payType.HasValue)
            {
                if (criteria.payType.Value == YeahTVApi.DomainModel.Enum.PayType.Movie)
                {
                    query = query.Where(q => (q.PayType == null || q.PayType == "") || q.PayType.ToLower().Trim().Equals(criteria.payType.Value.ToString().ToLower().Trim()));
                }
                else
                {

                    query =
                        query.Where(
                            q => (q.PayType != null && q.PayType != "") && q.PayType.ToLower().Trim().Equals(criteria.payType.Value.ToString().ToLower().Trim()));
                }
            }

            if (criteria.CompleteBeginTime != null)
                query = query.Where(q => q.CompleteTime >= criteria.CompleteBeginTime);

            if (criteria.CompleteEndTime != null)
                query = query.Where(q => q.CompleteTime <= criteria.CompleteEndTime);

            if (!string.IsNullOrEmpty(criteria.HotelId))
            {
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));
            }

            return query.ToPageList(criteria);
        }

        public string GetNewOrderId()
        {
            return string.Format("{0:yyMMddHHmmss}{1}", DateTime.Now, (base.Entities.Count() + 1).ToString().PadLeft(8, '0'));
        }
    }
}
