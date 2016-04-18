namespace YeahTVApiLibrary.Infrastructure
{
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.SearchCriteria;
    using System;
    using System.Collections.Generic;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.Common;

    public interface IOrderQRCodeRecordManager : IBaseManager<OrderQRCodeRecord, OrderQRCodeRecordCriteria>
    {
        string GetOrderIdByTicket(string ticket);
    }
}
