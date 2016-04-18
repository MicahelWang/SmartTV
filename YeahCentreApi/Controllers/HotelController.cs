using System.Collections.Generic;
using System.Web.Http;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahCenter.Infrastructure;
using YeahCentreApi.ViewModels;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Filter;
using System.Linq;
using YeahTVApi.Entity;
using System.Net.Http;
using System.Net;

namespace YeahCentreApi.Controllers
{
    public class HotelController : ApiController
    {

        // GET: api/Hotel/5
        private readonly IHotelManager _hotelManager;
        private readonly IHotelPermitionManager _hotelPermitionManager;

        public HotelController(IHotelManager hotelManager, IHotelPermitionManager hotelPermitionManager)
        {
            this._hotelManager = hotelManager;
            this._hotelPermitionManager = hotelPermitionManager;
        }



        [HttpGet]
        public HotelEntity Get(string id)
        {
            return _hotelManager.GetHotel(id);
        }

        [Route("api/Hotel/GetHotelByDeviceId")]
        [CenterApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = false)]
        [HttpPost]
        public ResponseData<HotelRoomEntity> GetHotelByDeviceId(PostParameters<PostHotelInfoData> request)
        {
            ResponseData<HotelRoomEntity> responseData = null;
            try
            {
                responseData = new ResponseData<HotelRoomEntity> { Sign = string.Empty, Data = _hotelManager.GetHotelByDeviceId(request.DeviceSeries) };
            }
            catch (ApiException ex)
            {
                var apiException = new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent(ex.Message), ReasonPhrase = ex.Message };
                throw new HttpResponseException(apiException);
            }
            return responseData;
        }

        [AcceptVerbs("DETAIL")]
        public HotelObject GetDetail(string id)
        {
            return _hotelManager.GetHotelObject(id);
        }

        [AcceptVerbs("ByBrand")]
        public IEnumerable<HotelEntity> GetByBrand(string id)
        {
            return _hotelManager.GetByBrand(id);
        }

        [AcceptVerbs("GetAll")]
        public IEnumerable<HotelEntity> GetAll()
        {
            return _hotelManager.GetAllHotels();
        }
        [AcceptVerbs("PutBaseData")]
        public string PutBaseData(string id,string baseData)
        {
             _hotelManager.UpdateDataBase(id,baseData);
            return "Success";
        }
        [AcceptVerbs("GetDataBase")]
        public string GetDataBase(string id)
        {
            return _hotelManager.GetDataBase(id);
        }
        //[AcceptVerbs("UpdateHotelData")]
        //public string UpdateHotelData(string id, string hotelDevice)
        //{
        //    _hotelManager.UpdateHotelData(id, hotelDevice);
        //    return "Success";
        //}
        [AcceptVerbs("GetByPageList")]
        public IPagedList<CoreSysHotel> GetByPageList(int pageIndex, int pageSize, string keyword)
        {
            return _hotelManager.PagedList(pageIndex, pageSize, keyword);
        }

        [AcceptVerbs("GetHotelDeviceInfo")]
        public string GetHotelDeviceInfo(string id)
        {
            return _hotelManager.GetHotelDeviceInfo(id);
           
        }
        // [AcceptVerbs("UpdateHotelDeviceDetail")]
        //public string UpdateHotelDeviceDetail(string hotelId,string hotelDeviceDetail)
        //{
        //    return _hotelManager.UpdateHotelDeviceDetail(hotelId,hotelDeviceDetail);
           
        //}
         [AcceptVerbs("GetHotelDeviceDetail")]
         public string GetHotelDeviceDetail(string id)
         {
             return _hotelManager.GetHotelDeviceDetail(id);

         }
         [AcceptVerbs("SearcherByPage")]
         public List<CoreSysHotel> SearcherByPage(int pageIndex, int pageSize, string keyword)
         {
             return _hotelManager.Search(
                 new CoreSysHotelCriteria() { 
                     Page = pageIndex
                     ,PageSize=20
                     ,NeedPaging=true
                     , HotelName=keyword
                     ,OrderAsc=false
                    ,SortFiled="CreateTime"
                    
                 });
         }


         [HttpPost]
         [Route("api/Hotel/GetHotelListByPermition")]
         [CenterApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = false)]
         public ResponseData<List<string>> GetHotelListByPermition(PostParameters<PermitionRequest> request)
         {
             var hotels = _hotelPermitionManager.GetHotelListByPermition(request.Data.UserId).Select(m=>m.Id).ToList();
             return new ResponseData<List<string>> { Data = hotels }; 
         }

    }
}
