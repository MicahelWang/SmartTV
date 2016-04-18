using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;

namespace YeahTVApiLibrary.Manager
{
    public class StoreOrderManager : IStoreOrderManager
    {
        private IStoreOrderRepertory storeOrderRepertory;
        public StoreOrderManager(IStoreOrderRepertory storeOrderRepertory)
        {
            this.storeOrderRepertory = storeOrderRepertory;
        }

        public void UpdateStoreOrder(StoreOrder storeOrder)
        {
            var StoreOrderDb = storeOrderRepertory.Search(new StoreOrderCriteria { Hotelid = storeOrder.Hotelid, Orderid = storeOrder.Id }).FirstOrDefault();

            StoreOrderDb.Status = storeOrder.Status;

            storeOrderRepertory.Update(StoreOrderDb);
        }

        public StoreOrder GetStoreOrder(StoreOrderCriteria criteria)
        {
            return storeOrderRepertory.Search(criteria).FirstOrDefault();
        }

        public void Update(StoreOrder entity)
        {
            storeOrderRepertory.Update(entity);
        }
        public void Add(StoreOrder entity)
        {
            storeOrderRepertory.Insert(entity);
        }


        public StoreOrder FindByKey(string orderId)
        {
            return storeOrderRepertory.FindByKey(orderId);
        }

        [UnitOfWork]
        public StoreOrder PaymentCallBack(PaymentInfo paymentInfo, PaymentCallBackData paymentCallBackData)
        {
            var order = storeOrderRepertory.Search(new StoreOrderCriteria() { Orderid = paymentCallBackData.ReturnInfo.OrderId }).FirstOrDefault();

            if (order == null || order.Status != (int)OrderState.Paying)
                throw new Exception("订单不存在或订单状态异常！");

            order.Status = (int)(paymentInfo.ResultCode.Equals(PayMentOrderState.Success.GetValueStr()) ? OrderState.Success : OrderState.Fail);
            order.CompleteTime = DateTime.Now;
            storeOrderRepertory.Update(order);

            return order;
        }

        public List<StoreOrder> SearchStoreOrder(StoreOrderCriteria storeOrderCriterias)
        {
            return storeOrderRepertory.Search(storeOrderCriterias);
        }

        public string GetNewOrderId(string hotelCode,string orderType="001")
        {
            return storeOrderRepertory.GetNewOrderId(hotelCode, orderType);
        }
    }
}


