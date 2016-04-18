namespace YeahTVApi.ServiceProvider
{
    using HZ.Web.Authorization;
    using YeahTVApi.Common;
    using YeahTVApi.Entity;
    using YeahTVApi.Entity.CentralMapping;
    using YeahTVApi.Infrastructure;
    using YeahTVApiLibrary.Infrastructure;
    using System;
    using System.Collections.Generic;

    public class HotelListService : CentralGetwayServiceBase, IHotelListService
    {
        private ILogManager logManager;

         /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public HotelListService(ILogManager logManager)
            : base(typeof(CentralApiResult<QueryHotelResult>))
        {
            this.logManager = logManager;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <param name="guest"></param>
        /// <returns></returns>
        public override Object ConvertTo(string json, Guest guest)
        {
            var result = base.ConvertTo(json, guest);
            if (result == null)
                return null;
            var data = result as CentralApiResult<QueryHotelResult>;
            if (data == null)
                return null;
            if (data.ResultType != Entity.CentralMapping.OperationResultType.Successed)
            {
                throw new ApiException(data.Message);
            }

            return data.Data.HotelList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="guest"></param>
        /// <param name="HotelID"></param>
        /// <param name="CheckInDate"></param>
        /// <param name="CheckOutDate"></param>
        /// <returns></returns>
        public List<Hotel> Query(List<string> HotelIDs, String CheckInDate, String CheckOutDate)
        {
            List<KeyValuePair<String, String>> pams = new List<KeyValuePair<String, String>>();
            pams.Add(new KeyValuePair<string, string>("CheckInDate", CheckInDate));
            pams.Add(new KeyValuePair<string, string>("CheckOutDate", CheckOutDate));

            HotelIDs.ForEach(h =>
            {
                pams.Add(new KeyValuePair<string, string>("HotelIDList", h));
            });

            pams.Add(new KeyValuePair<string, string>("QueryRoomType", "NoSet"));
            pams.Add(new KeyValuePair<string, string>("IsGetVirtualRoom", "False"));

            //完成用户在大促销环境中的登录操作。
            var hotels = new List<Hotel>();
            try
            {
                var response = CentralApi.GetResponse(APICallFactory.CallAction(APICallFactory.APICallType.HotelList), pams, null, null);
                var responseJson = response.Content.ReadAsStringAsync().Result;
                response.Dispose();
                response = null;
                JsonString = responseJson;
                var hotelsResult = ConvertTo(responseJson, null) as List<HotelViewModel>;

                hotelsResult.ForEach(h =>
                {
                    hotels.Add(GenerateHotel(h));
                });
            }
            catch (Exception err)
            {
                logManager.SaveError(this.GetType().Name, err, DomainModel.Enum.AppType.TV);
            }

            return hotels;
        }

        private Hotel GenerateHotel(HotelViewModel model)
        {
            //内宾修正
            var cityCode = model.Info.CityID.ToString();
            var hotel = new Hotel
            {
                hotelID = model.Info.ID,
                hotelName = model.Info.HotelName,
                cityCode = cityCode,
                cityName = model.Info.CityName,
                address = model.Info.Address,

                geoInfo = model.Info.Lat + "|" + model.Info.Lng,
                commentScore = model.Info.CommentScore,
                commentCount = model.Info.CommentCount,
                tel = model.Info.Telephone,
                URL360 = model.Navigate360,
                IsCanCardPay = model.Info.IsSupportValueCardPay,
                IsOpenCheckIn = model.Info.IsOpenOnlineCheckIn,
            };

            //提出名称里面的汉庭字样
            hotel.hotelName = hotel.hotelName.Replace("汉庭酒店", "");
            hotel.hotelName = hotel.hotelName.Replace("全季酒店", "");
            hotel.hotelName = hotel.hotelName.Replace("海友酒店", "");
            hotel.hotelName = hotel.hotelName.Replace("星程酒店", "");
            hotel.hotelStyle = model.Info.HotelStyle.ToString();
            hotel.hotelStyleName = model.Info.HotelStyle.GetHotelStyleName();
            hotel.Images = new List<Entity.HotelImage>();
            
            if (model.Info.HotelImage != null)
            {

                foreach (var item in model.Info.HotelImage)
                {
                    Entity.HotelImage image = new Entity.HotelImage();
                    image.ImageURL = item.ImageUrl;
                    image.ImageName = item.ImageName;
                    image.Desc = item.ImageDesc;
                    hotel.Images.Add(image);
                }
            }

            hotel.BrandTitle = PubFun.GetAppSetting(hotel.hotelStyle + "_TITLE");
            hotel.BrandDescription = PubFun.GetAppSetting(hotel.hotelStyle + "_DESC");

            #region 服务设施
            if (model.Info.HotelFacility != null)
            {
                hotel.services = new List<HotelService>();
                foreach (HotelFacilityViewModel entity in model.Info.HotelFacility)
                {
                    HotelService service = new HotelService();
                    service.FacilityID = entity.FacilityID.ToString();
                    service.FacilityName = entity.Name;
                    service.TypeID = entity.FacilityTypeID.ToString();
                    service.TypeName = entity.FacilityTypeName;
                    service.Descript = entity.Descript;
                    hotel.services.Add(service);
                }
            }
            #endregion

            return hotel;
        }
    }
}
