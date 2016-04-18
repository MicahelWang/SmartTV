using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Entity;
using YeahTVApi.Infrastructure;
using YeahTVApi.ServiceProvider.PMS;

namespace YeahTVApi.ServiceProvider
{
    public class HotelCommodityService : CentralGetwayServiceBase, IHotelCommodityService
    {
        public HotelCommodityService()
            : base(null)
        {
        }

        /// <summary>
        /// 获取酒店可售小商品列表
        /// </summary>
        /// <param name="hotelID"></param>
        public Tuple<Dictionary<string, List<HotelCommodityEntity>>, List<HotelCommodityCategoryEntity>> GetProductListByHotelId(string hotelID)
        {
            var client = new PMS.PmsServiceClient();
            var list = client.GetProductListByHotelId(hotelID).OrderBy(t => t.CategoryId);
            //小商品对应集合
            var dictionary = new Dictionary<string, List<HotelCommodityEntity>>();
            //小商品目录
            var listCategoryEntities = new List<HotelCommodityCategoryEntity>();
            var tuple = new Tuple<Dictionary<string, List<HotelCommodityEntity>>, List<HotelCommodityCategoryEntity>>(dictionary, listCategoryEntities);
            string lastCategoryCode = "", categoryCode = "";
            var listEntity = new List<HotelCommodityEntity>();
            foreach (var vProductEntity in list)
            {
                categoryCode = vProductEntity.CategoryCode;
                var hotelCommodityEntity = convertToCommodityEntity(vProductEntity);
                if (lastCategoryCode != categoryCode)
                {
                    listCategoryEntities.Add(hotelCommodityEntity);
                    if (lastCategoryCode != "")
                    {
                        dictionary.Add(lastCategoryCode, listEntity);
                        listEntity = new List<HotelCommodityEntity>();
                    }
                }
                listEntity.Add(hotelCommodityEntity);
                lastCategoryCode = categoryCode;
            }
            if (categoryCode != "")
            {
                dictionary.Add(categoryCode, listEntity);
            }
            return tuple;
        }

        /// <summary>
        /// 小商品对象转换
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private HotelCommodityEntity convertToCommodityEntity(V_ProductEntity entity)
        {
            HotelCommodityEntity commodityEntity = new HotelCommodityEntity();
            commodityEntity.CategoryId = entity.CategoryId.ToString();
            commodityEntity.CategoryName = entity.CategoryName;
            commodityEntity.CategoryCode = entity.CategoryCode;
            commodityEntity.Name = entity.Name;
            commodityEntity.MarketingPrice = entity.MarketingPrice;
            commodityEntity.ProductId = entity.ProductId.ToString();
            commodityEntity.Id = entity.Id.ToString();
            commodityEntity.UOM = entity.UOM;
            return commodityEntity;

        }

        /// <summary>
        /// 商品目录转换
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private HotelCommodityCategoryEntity convertToCommodityCategoryEntity(HotelCommodityEntity entity)
        {
            HotelCommodityCategoryEntity categoryEntity = new HotelCommodityCategoryEntity();
            categoryEntity.CategoryId = entity.CategoryId.ToString();
            categoryEntity.CategoryName = entity.CategoryName;
            categoryEntity.CategoryCode = entity.CategoryCode;
            return categoryEntity;
        }

        /// <summary>
        /// 提交小商品订单
        /// </summary> 
        public bool SubmitProductSalesOrder(string hotelID, string salesDetails, out string salesOrderNo, out string errMsg)
        {
            bool flag = false;
            var client = new PMS.PmsServiceClient(); 
            flag = client.SubmitProductSalesOrder(hotelID, salesDetails, out salesOrderNo, out errMsg);
            return flag;
        }
        /// <summary>
        /// 提交财务
        /// </summary> 
        public bool ChargeRoomAccountByBillId(string salesOrderNo, decimal amount, string hotelID, string roomNo, string cusName, out string errMsg)
        {
            throw new NotImplementedException();
        }
    }
}
