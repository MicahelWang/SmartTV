using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;
using YeahCenter.Infrastructure;
using YeahCentreApi.ViewModels;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahCentreApi.Controllers
{
    [RoutePrefix("api/CommodityClassification")]
    public class CommodityClassificationController : ApiController
    {
        private ILogManager logManager;
        private readonly IHotelManager _hotelManager;
        private ITVHotelConfigManager tvHotelConfigManager;
        private IDeviceTraceLibraryManager _DeviceTraceManager;
        private IConstantSystemConfigManager _constantSystemConfigManager;
        private IRequestApiService requestApiService;
        private IStoreOrderManager storeOrderManager;
        private IAppLibraryManager AppManager;
        public CommodityClassificationController(
            IHotelManager hotelManager,
            IDeviceTraceLibraryManager DeviceTraceManager,
            IConstantSystemConfigManager constantSystemConfigManager,
            ILogManager logManager,
             IRequestApiService requestApiService,
            IStoreOrderManager storeOrderManager,
            ITVHotelConfigManager tvHotelConfigManager,
            IAppLibraryManager AppManager
            )
        {
            this._hotelManager = hotelManager;
            this._DeviceTraceManager = DeviceTraceManager;
            this._constantSystemConfigManager = constantSystemConfigManager;
            this.logManager = logManager;
            this.requestApiService = requestApiService;
            this.storeOrderManager = storeOrderManager;
            this.tvHotelConfigManager = tvHotelConfigManager;
            this.AppManager = AppManager;

        }

        #region 获取商品分类接口
        [HttpPost]
        [Route("CategoryAction")]
        [CenterApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = true)]
        public ResponseData<CategoryReturnData> CategoryAction(PostParameters<string> request)
        {
            string url = _constantSystemConfigManager.ShoppingMallUrl + "/HotelMall/webapi/getCategoryList.do";//
            PostParameters<ReqestCommodityCate> data = CreateSignAndDataMethod(request.DeviceSeries);
            PostParameters<CategoryReturnData> returnCategoryList = new PostParameters<CategoryReturnData>();
            StorePayConfig hotelPayment = new StorePayConfig();
            try
            {
                ResponseData<List<CommdityCategory>> returnData = JsonConvert.DeserializeObject<ResponseData<List<CommdityCategory>>>
                ((new HttpHelper() { ContentType = "application/json" }).Post(url, JsonConvert.SerializeObject(data)));
                BackupDevice backupDevice = null;
                HotelEntity hotel = null;
                if (returnData != null)
                {
                    var trace = CreateHeaderMethod(request.DeviceSeries);
                    if (trace != null)
                    {
                        hotel = GetHotelInfo(trace.HotelId);
                    }
                    else
                    {
                        var header = GetRequestHeader(request.DeviceSeries);
                        backupDevice = AppManager.GetAppBackupDevice(header);
                        hotel = GetHotelInfo(backupDevice.HotelId);
                    }
                    hotelPayment = GetSendType(hotel.HotelId);
                    returnCategoryList.Data = new CategoryReturnData
                    {
                        Categorys = returnData.Data,
                        SentRoom = hotelPayment.SentRoom.Value
                    };

                }
                return new ResponseData<CategoryReturnData> { Data = returnCategoryList.Data };
            }
            catch (Exception e)
            {
                logManager.SaveError(string.Format("分类信息错误,设备号为：{0}",
                    request.DeviceSeries), null, AppType.YeahCenterApi, null);
                throw new Exception(string.Format("异常信息{0}", e.Message));
            }
        }
        #endregion

        #region 商品详情接口
        [HttpPost]
        [Route("CommdityInfoAction")]
        [CenterApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = true)]
        public ResponseData<object> CommdityInfoAction(PostParameters<CommdityInfo> request)
        {
            string url = _constantSystemConfigManager.ShoppingMallUrl + "/HotelMall/webapi/getGoodsList.do";//
            PostParameters<CommdityInfo> data = request;
            try
            {
                ResponseData<GoodsInfoList> returnData = JsonConvert.DeserializeObject<ResponseData<GoodsInfoList>>((
                new HttpHelper() { ContentType = "application/json" }).Post(url, JsonConvert.SerializeObject(data)));
                ResponseData<CommodityDetialList> responseCom = CreateCommodityInfo(request, returnData);
                responseCom.Sign = CreateSignMethod(responseCom.Data);
                return new ResponseData<object> { Sign = responseCom.Sign, Data = responseCom.Data };
            }
            catch (Exception e)
            {
                logManager.SaveError(string.Format("商品列表信息异常,设备号为：{0}",
                    request.DeviceSeries), null, AppType.YeahCenterApi, null);
                throw new Exception(string.Format("异常信息{0}", e.Message));
            }
        }

        #endregion

        #region 确认下单接口
        [HttpPost]
        [Route("OrderInfoAction")]
        [CenterApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = true)]
        public ResponseData<CreateOrderApiResult> OrderInfoAction(PostParameters<ProductList> request)
        {
            StringBuilder qutityNumStr = new StringBuilder();
            StringBuilder offShelf = new StringBuilder();
            HotelEntity hotel = null;
            string roomNum;
            BackupDevice backupDevice = null;
            CreateOrderApiResult err = new CreateOrderApiResult();
            ResponseData<List<ProductMinusOrAdd>> productMAs = new ResponseData<List<ProductMinusOrAdd>>();
            List<ProductMinusOrAdd> productMinList = new List<ProductMinusOrAdd>();
            string commodityQ = null;
            bool hasErr = false;

            string url = _constantSystemConfigManager.ShoppingMallUrl;//
            var trace = CreateHeaderMethod(request.DeviceSeries);

            commodityQ = CheckQuantity(request);
            if (!string.IsNullOrEmpty(commodityQ))
            {
                return new ResponseData<CreateOrderApiResult>()
                {
                    Data = err.WithError(commodityQ,
                    trace != null ? trace.RoomNo : "", int.Parse(CommodityErrorType.Abnormal.GetValueStr()))
                };
            }

            PostParameters<GoodIdArray> goods = new PostParameters<GoodIdArray>()
            {
                Data = new GoodIdArray()
                {
                    GoodsIds = request.Data.Products.Select(m => m.ProductId).ToList()
                }
            };
            goods.Sign = CreateSignMethod(goods.Data);
            ResponseData<List<GoodsInfomation>> returnData = JsonConvert.DeserializeObject<ResponseData<List<GoodsInfomation>>>((
                new HttpHelper() { ContentType = "application/json" }).
                Post(url + "/HotelMall/webapi/getGoodsInfo.do", JsonConvert.SerializeObject(goods)));


            returnData.Data.ForEach(m =>
            {
                if (!goods.Data.GoodsIds.Contains(m.Id))
                {
                    hasErr = true;
                    return;
                }
            });
            if (hasErr)
                return new ResponseData<CreateOrderApiResult>()
                {
                    Data = err.WithError(CommodityErrorType.Abnormal.ToString(),
                    trace != null ? trace.RoomNo : "", int.Parse(CommodityErrorType.Abnormal.GetValueStr()))
                };

            var exceptGoods = goods.Data.GoodsIds.Except(returnData.Data.Select(m => m.Id)).ToList();
            if (exceptGoods.Count > 0)
            {
                return new ResponseData<CreateOrderApiResult>()
                {
                    Data = err.WithError(string.Join("|", exceptGoods).TrimEnd('|'),
                    trace != null ? trace.RoomNo : "", int.Parse(CommodityErrorType.Deleted.GetValueStr()))
                };
            }

            request.Data.Products.ForEach(m =>
            {
                GoodsInfomation goodInfo = returnData.Data.Single(h => h.Id.Equals(m.ProductId));
                if (goodInfo != null)
                {
                    if (Convert.ToInt32(goodInfo.Quantity) < Convert.ToInt32(m.Quantity))
                        qutityNumStr.Append(goodInfo.Id + "|" + goodInfo.Quantity + ";");
                    if (!Convert.ToBoolean(goodInfo.OnSale))
                    {
                        offShelf.Append(goodInfo.Id + "|" + goodInfo.Image + "|" + goodInfo.Name + ";");
                    }
                }
            });

            if (!string.IsNullOrWhiteSpace(qutityNumStr.ToString()))
                return new ResponseData<CreateOrderApiResult>()
                {
                    Data = err.WithError(qutityNumStr.ToString(), trace != null ? trace.RoomNo : "", Convert.ToInt32(CommodityErrorType.Lack))
                };

            if (!string.IsNullOrWhiteSpace(offShelf.ToString()))
                return new ResponseData<CreateOrderApiResult>()
                {
                    Data = err.WithError(offShelf.ToString(), trace != null ? trace.RoomNo : "", Convert.ToInt32(CommodityErrorType.OffShelf))
                };

            request.Data.Products.ForEach(m =>
            {
                productMinList.Add(new ProductMinusOrAdd
                                     {
                                         ProductId = m.ProductId,
                                         Quantity = m.Quantity,
                                         Type = ProductType.MINUS.ToString()
                                     });
            });
            productMAs.Data = productMinList;
            productMAs.Sign = CreateSignMethod(productMAs.Data);

            ResponseData<MinusCommodityMessage> minusComMessage = new ResponseData<MinusCommodityMessage>();
            int requestCount = 0;
            do
            {
                requestCount++;
                minusComMessage = JsonConvert.DeserializeObject<ResponseData<MinusCommodityMessage>>((new HttpHelper() { ContentType = "application/json" })
                    .Post(url + "/HotelMall/webapi/updateGoodsQuantity.do", JsonConvert.SerializeObject(productMAs)));
            }
            while (!minusComMessage.Data.Success && requestCount < 3);

            if (!minusComMessage.Data.Success)
            {
                //ERROR log
                logManager.SaveError(minusComMessage.Data.ErrorMsg, null, AppType.YeahCenterApi, null);
            }
            if (trace != null)
            {
                hotel = GetHotelInfo(trace.HotelId);
                roomNum = trace.RoomNo;
            }

            else
            {
                var header = GetRequestHeader(request.DeviceSeries);
                backupDevice = AppManager.GetAppBackupDevice(header);
                hotel = GetHotelInfo(backupDevice.HotelId);
                roomNum = "";
            }
            StorePayConfig hotelPayment = GetSendType(hotel.HotelId);
            string orderId = storeOrderManager.GetNewOrderId(hotel.HotelCode);
            bool IsInsertSuc = InsertOrderData(request.DeviceSeries, roomNum, returnData.Data, hotel, orderId, hotelPayment.SentRoom.Value, request.Data.Products);
            if (!IsInsertSuc)
            {
                logManager.SaveError(JsonConvert.SerializeObject(returnData),
                    "订单号为：" + orderId + "的数据插入失败", AppType.YeahCenterApi, null);
            }
            return new ResponseData<CreateOrderApiResult>
            {
                Data = err.WithOk(orderId, roomNum, hotelPayment.SentRoom.Value
                , int.Parse(CommodityErrorType.Success.GetValueStr()))
            };
        }

        private static string CheckQuantity(PostParameters<ProductList> returnData)
        {
            StringBuilder commodityQ = new StringBuilder();
            returnData.Data.Products.ForEach(m =>
            {
                if (m.Quantity <= 0)
                {
                    commodityQ.Append(m.ProductId + ";");
                }
            });
            return commodityQ.ToString().TrimEnd(';');
        }

        #endregion

        #region 获取商城支付信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="hotel"></param>
        /// <returns></returns>
        private StorePayConfig GetSendType(string hotelId)
        {

            var hotelPaymentInfo = tvHotelConfigManager.GetHotelConfig(
                new HotelConfigCriteria() { HotelId = hotelId, ConfigCodes = "StorePaymentModel" });

            if (hotelPaymentInfo == null)
            {
                logManager.SaveError(string.Format("没有找到酒店Id为{0}的支付配置信息", hotelId),
                    "配置名称为：StorePaymentModel", AppType.YeahCenterApi);
                throw new Exception("没有找到" + hotelId + "支付配置信息");
            }

            var hotelPayment = new StorePayConfig();
            try
            {
                hotelPayment = JsonConvert.DeserializeObject<StorePayConfig>(hotelPaymentInfo.ConfigValue);
            }
            catch (Exception ex)
            {
                hotelPayment.SentRoom = true;
            }
            return hotelPayment;
        }
        #endregion

        #region 根据设备号获取酒店信息
        private HotelEntity GetHotelInfo(string hotelId)
        {
            var requestHotelUrl = _constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + hotelId;
            var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();
            return hotel;
        }
        #endregion

        #region 订单数据入库方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="trace"></param>
        /// <param name="goodsInfos"></param>
        /// <param name="hotel"></param>
        /// <param name="orderId"></param>
        /// <param name="deliveryType"></param>
        /// <param name="Products"></param>
        /// <returns></returns>
        private bool InsertOrderData(string deviceSeries, string roomNum, List<GoodsInfomation> goodsInfos, HotelEntity hotel,
            string orderId, bool deliveryType, List<Product> Products)
        {
            decimal sumPrice = 0;
            goodsInfos.ForEach(m =>
            {
                sumPrice = sumPrice + m.Price * Products.Single(h => h.ProductId == m.Id).Quantity;
            });
            var storeOrder = new StoreOrder()
            {
                Id = orderId,
                CreateTime = DateTime.Now,
                CompleteTime = DateTime.Now,
                Hotelid = hotel.HotelId,
                HotelName = hotel.HotelName,
                RoomNo = roomNum,
                SeriseCode = deviceSeries,
                Price = sumPrice,
                Status = (int)OrderState.Unpaid,
                GoodsName = string.Format("{0}小商品", hotel.HotelName),
                IsDelete = false,
                PayInfo = "",
                DeliveryState = 0,
                ExpirationDate = DateTime.Now.AddMinutes(_constantSystemConfigManager.ExpirationDate),
                GoodsDesc = "",
                DeliveryType = deliveryType ? DeliveryType.SentRoom.ToString() : DeliveryType.ReceptionDesk.ToString(),
                OrderProducts = goodsInfos.Select(m => new OrderProducts()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    CreateTime = DateTime.Now,
                    OrderId = orderId,
                    ProductId = m.Id,
                    ProductInfo = JsonConvert.SerializeObject(m),
                    ProductName = m.Name,
                    Quantity = Products.Single(h => h.ProductId == m.Id).Quantity,
                    UnitPrice = m.Price,
                }).ToList(),
            };
            try
            {
                storeOrderManager.Add(storeOrder);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        #endregion

        #region 商品分类Data数据创建
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceSeries"></param>
        /// <returns></returns>
        /// 
        private PostParameters<ReqestCommodityCate> CreateSignAndDataMethod(string deviceSeries)
        {
            BackupDevice backupDevice = null;
            var trace = CreateHeaderMethod(deviceSeries);
            if (trace == null)
            {
                var header = GetRequestHeader(deviceSeries);
                backupDevice = AppManager.GetAppBackupDevice(header);
            }
            PostParameters<ReqestCommodityCate> returnData = new PostParameters<ReqestCommodityCate>();
            returnData.Data = new ReqestCommodityCate
            {
                HotelId = trace == null ? backupDevice.HotelId : trace.HotelId
            };
            returnData.Sign = CreateSignMethod(returnData.Data);
            return returnData;

        }
        #endregion

        #region 签名生成方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string CreateSignMethod(object data)
        {
            string sign = new StringBuilder().
                Append(JsonConvert.SerializeObject(data)).
                Append(_constantSystemConfigManager.StoreSignPrivateKey)
                .ToString().StringToMd5();
            return sign;
        }
        #endregion

        #region 设备信息获取方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceSeries"></param>
        /// <returns></returns>
        private DeviceTrace CreateHeaderMethod(string deviceSeries)
        {
            var header = GetRequestHeader(deviceSeries);
            var trace = _DeviceTraceManager.GetAppTrace(header);
            return trace;
        }

        private static RequestHeader GetRequestHeader(string deviceSeries)
        {
            var header = new RequestHeader
            {
                DEVNO = deviceSeries,
                APP_ID = "deviceSeries"
            };
            return header;
        }
        #endregion

        #region 根据商品Id列表获取商品列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="returnData"></param>
        /// <returns></returns>
        private static ResponseData<CommodityDetialList> CreateCommodityInfo(PostParameters<CommdityInfo> request, ResponseData<GoodsInfoList> returnData)
        {
            ResponseData<CommodityDetialList> responseCom = new ResponseData<CommodityDetialList>()
            {
                Data = new CommodityDetialList
                {
                    PageIndex = request.Data.Pageindex,
                    PageSize = request.Data.Pagesize,
                    TotalPage = returnData.Data.PageTotal,
                    Products = returnData.Data.Goods.Select(m => new ProductLists
                    {
                        brand = m.Brand,
                        description = m.Description,
                        image_url = m.Image,
                        name = m.Name,
                        number = m.Number,
                        on_sale = m.OnSale,
                        price = m.Price.ToString(),
                        quantity = m.Quantity,
                        specification = m.Specification,
                        stock_taking_time = m.Stock_taking_time,
                        unit = m.Unit,
                        id = m.Id

                    }).ToList(),
                }
            };
            return responseCom;
        }
        #endregion
    }
}
