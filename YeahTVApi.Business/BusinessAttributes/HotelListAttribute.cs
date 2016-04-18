using HZ.Web.Authorization;
using HZTVApi.Common;
using HZTVApi.Entity;
using HZTVApi.Entity.CentralMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HZTVApi.Business.BusinessAttributes
{
    /// <summary>
    /// 酒店详情转换类
    /// </summary>
    public class HotelListAttribute : BusinessAttribute
    {
        /// <summary>
        /// 设置源需要做反序列化的对象类型
        /// </summary>
        /// <param name="sourceType"></param>
        public HotelListAttribute()
            : base(typeof(CentralApiResult<QueryHotelResult>))
        {

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
                HTOutputLog.SaveError(this.GetType().Name, err);
            }

            return hotels;
        }


        private Hotel GenerateHotel(HotelViewModel model)
        {

            //dictAreaName = HotelManager.GetHotelAreaName(id_list.ToString());
            //内宾修正
            string cityCode = model.Info.CityID.ToString();
            Hotel htl = new Hotel
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
            htl.hotelName = htl.hotelName.Replace("汉庭酒店", "");
            htl.hotelName = htl.hotelName.Replace("全季酒店", "");
            htl.hotelName = htl.hotelName.Replace("海友酒店", "");
            htl.hotelName = htl.hotelName.Replace("星程酒店", "");
            htl.hotelStyle = model.Info.HotelStyle.ToString();
            htl.hotelStyleName = model.Info.HotelStyle.GetHotelStyleName();
            htl.Images = new List<Entity.HotelImage>();
            if (model.Info.HotelImage != null)
            {

                foreach (var item in model.Info.HotelImage)
                {
                    Entity.HotelImage image = new Entity.HotelImage();
                    image.ImageURL = item.ImageUrl;
                    image.ImageName = item.ImageName;
                    image.Desc = item.ImageDesc;
                    htl.Images.Add(image);
                }
            }

            htl.BrandTitle = PubFun.GetAppSetting(htl.hotelStyle + "_TITLE");
            htl.BrandDescription = PubFun.GetAppSetting(htl.hotelStyle + "_DESC");
            #region 服务设施
            if (model.Info.HotelFacility != null)
            {
                htl.services = new List<HotelService>();
                foreach (HotelFacilityViewModel entity in model.Info.HotelFacility)
                {
                    HotelService service = new HotelService();
                    service.FacilityID = entity.FacilityID.ToString();
                    service.FacilityName = entity.Name;
                    service.TypeID = entity.FacilityTypeID.ToString();
                    service.TypeName = entity.FacilityTypeName;
                    service.Descript = entity.Descript;
                    htl.services.Add(service);
                }
            }
            #endregion

            return htl;

        }


    }

}
