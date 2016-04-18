using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;
using YeahTVApi.DomainModel.Enum;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class StoreOrderRepertory : BaseRepertory<StoreOrder, string>, IStoreOrderRepertory
    {
        public override List<StoreOrder> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as StoreOrderCriteria;

            var query = base.Entities.Include("OrderProducts").AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Hotelid))
                query = query.Where(q => q.Hotelid.Equals(criteria.Hotelid));

            if (!string.IsNullOrEmpty(criteria.Orderid))
                query = query.Where(q => q.Id.Equals(criteria.Orderid));

            if (criteria.Status.HasValue)
                query = query.Where(q => q.Status.Equals(criteria.Status.Value));

            if (criteria.IsDelete.HasValue)
                query = query.Where(q => q.IsDelete == (criteria.IsDelete.Value));

            if (criteria.Begindate.HasValue)
                query = query.Where(q => q.CreateTime >= criteria.Begindate.Value);

            if (criteria.Enddate.HasValue)
                query = query.Where(q => q.CreateTime <= criteria.Enddate.Value);

            if (!string.IsNullOrWhiteSpace(criteria.Roomnumber))
                query = query.Where(q => q.RoomNo == criteria.Roomnumber);

            switch (criteria.Transactionstate)
            {
                case Transactionstate.Cancel:
                    query = query.Where(m => m.Status == (int)OrderState.Cancel || ((m.Status == (int)OrderState.Unpaid || m.Status == (int)OrderState.Paying || m.Status==(int)OrderState.Fail) && DateTime.Now > m.ExpirationDate && m.PayInfo.ToLower().Trim() != PayPaymentModel.QTPAY.ToString().ToLower().Trim()));
                    break;
                case Transactionstate.Paid:
                    query = query.Where(m => m.Status == (int)OrderState.Success && m.DeliveryState == (int)DeliveryState.UnDelivery);
                    break;
                case Transactionstate.Transactionscomplete:
                    query = query.Where(m => m.Status == (int)OrderState.Success && m.DeliveryState == (int)DeliveryState.Delivery);
                    break;
                case Transactionstate.Unpaid:
                    query = query.Where(m => (m.Status == (int)OrderState.Fail || m.Status == (int)OrderState.Unpaid || m.Status == (int)OrderState.Paying) && DateTime.Now <= m.ExpirationDate && m.PayInfo.ToLower().Trim()!= PayPaymentModel.QTPAY.ToString().ToLower().Trim());
                    break;
                case Transactionstate.Waiting:
                    query = query.Where(m => (m.Status == (int)OrderState.Paying || m.Status == (int)OrderState.Unpaid  || m.Status == (int)OrderState.Fail) && m.PayInfo.ToLower() == PayPaymentModel.QTPAY.ToString().ToLower());
                    break;
            }

            return query.ToPageList(searchCriteria);
        }

        public string GetNewOrderId(string hotelCode, string orderType)
        {
            return string.Format("{0}{1}{2:yyMMddHHmmss}{3}", orderType.PadLeft(3, '0'), hotelCode.PadLeft(9, '0'), DateTime.Now, (base.Entities.Count() + 1).ToString().PadLeft(8, '0'));
        }
    }
}
