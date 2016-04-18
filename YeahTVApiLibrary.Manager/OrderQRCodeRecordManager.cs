namespace YeahTVLibrary.Manager
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel.Mapping;
    using System;
    using YeahTVApi.DomainModel;
    using YeahTVApiLibrary.Manager;
    using YeahTVApi.DomainModel.Models.ViewModels;

    public class OrderQRCodeRecordManager : BaseManager<OrderQRCodeRecord, OrderQRCodeRecordCriteria>, IOrderQRCodeRecordManager
    {
        private IOrderQRCodeRecordRepertory orderQRCodeRecordRepertory;

        public OrderQRCodeRecordManager(IOrderQRCodeRecordRepertory orderQRCodeRecordRepertory)
            : base(orderQRCodeRecordRepertory)
        {
            this.orderQRCodeRecordRepertory = orderQRCodeRecordRepertory;
        }

        public string GetOrderIdByTicket(string ticket)
        {
            var record = orderQRCodeRecordRepertory.Search(new OrderQRCodeRecordCriteria() { Ticket = ticket }).FirstOrDefault();
            return record == null ? "" : record.OrderId;
        }

    }
}
