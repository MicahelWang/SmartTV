using System;

namespace YeahTVApi.Infrastructure
{
    using YeahTVApi.Entity;
    using YeahTVApi.Entity.CentralMapping;
    using System.Collections.Generic;

    public interface IHotelCommodityService : ICentralGetwayServiceBase
    {
        /// <summary>
        /// 获取酒店可售小商品列表
        /// 参数：酒店ID
        ///返回：酒店可销售小商品列表
        ///备注：销售价字段是MarketingPrice
        /// </summary> 
        Tuple<Dictionary<string, List<HotelCommodityEntity>>, List<HotelCommodityCategoryEntity>>
       GetProductListByHotelId(string hotelID);

        /// <summary>
        ///提交小商品订单
        ///参数：酒店ID、操作员、销售清单(多个商品"|"相隔，格式如：[GoodsId],[销售价],[数量]|...)、订单号(返回值)、错误信息(返回值，入账失败是有值)
        ///返回：是否提交成功 
        /// </summary> 
        bool SubmitProductSalesOrder(string hotelID, string salesDetails, out string salesOrderNo, out string errMsg);
        /// <summary>
        /// 提交财务 
        /// 参数：订单号、总金额、操作员、酒店ID、房间号、客户名称、错误信息(返回值，入账失败是有值)
        /// 返回：是否提交成功
        /// </summary>  
        bool ChargeRoomAccountByBillId(string salesOrderNo, decimal amount, string hotelID, string roomNo, string cusName, out string errMsg);
    }
}
