using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure.ManagerInterface
{
  public interface IStoreOrderManager
    {
      void UpdateStoreOrder(StoreOrder storeOrder);
      void Update(StoreOrder entity);
      void Add(StoreOrder entity);
      StoreOrder FindByKey(string orderId);
      StoreOrder PaymentCallBack(PaymentInfo paymentInfo, PaymentCallBackData paymentCallBackData);

      StoreOrder GetStoreOrder(StoreOrderCriteria criteria);

      List<StoreOrder> SearchStoreOrder(StoreOrderCriteria storeOrderCriteria);
      string GetNewOrderId(string hotelCode, string orderType="001");
    }
}
