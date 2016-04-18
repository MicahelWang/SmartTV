using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using YeahTVApi.Client.Models;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApi.Client.Controllers
{
    public class MockRequestController : Controller
    {
        private const string ResultKey = "TVUOqmnsnS8Oq8RWhuC93rKbR09T242V";
        List<SelectListItem> urlItemList = new List<SelectListItem>() {     
                new SelectListItem(){
                 Text="=可选择预置Url=",
                 Value=""
                },        
                new SelectListItem(){
                 Text="https://{host}/app/AddBehaviorLog",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/app/GetDeviceApps",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/app/start",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/app/GetDeviceApps",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/app/AddBehaviorLog",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/MovieTVChanelsResources/GetHotelMovies",
                 Value=""
                },new SelectListItem(){
                 Text=" https://{host}/MovieTVChanelsResources/GetHotelChanels",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/MovieTVChanelsResources/GetVODRecordByChargeNo",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/Hotel/GetHotelDetail",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/Hotel/GetWeather",
                 Value=""
                }  ,new SelectListItem(){
                 Text="https://{host}/Payment/VodRequest",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/Payment/VodPayState",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/MovieTVChanelsResources/GetTemplateMovieAndPrice",
                 Value=""
                },new SelectListItem(){
                 Text="https://{host}/MovieTVChanelsResources/GetTemplateListModel",
                 Value=""
                }                     
            };
        // GET: MockRequest
        public ActionResult Index()
        {
            ViewBag.UrlList = urlItemList;
            var model = new MockRequestModel()
            {
                SecureKey = "3e7e1457-df2e-11e4-91d9-6c92bf08396s",
                AppKey = "3e7e1457-df2e-11e4-91d9-6c92bf08396d",
                APP_ID = "3e7e1457-df2e-11e4-91d9-6c92bf083967",
                Header = "{\\\"Brand\\\":\\\"HotelVision\\\",\\\"DEVNO\\\":\\\"b8:88:e3:00:04:8f\\\",\\\"IP\\\":\\\"192.168.8.23\\\",\\\"Manufacturer\\\":\\\"HotelVision\\\",\\\"Model\\\":\\\"HotelVision-AM-8726\\\",\\\"OSVersion\\\":\\\"4.2.2\\\",\\\"PackageName\\\":\\\"com.YeahInfo.Tv\\\",\\\"ScreenDpi\\\":160,\\\"ScreenHeight\\\":720,\\\"ScreenWidth\\\":1280,\\\"Ver\\\":\\\"1\\\"}"
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(MockRequestModel model)
        {
            try
            {
                ViewBag.UrlList = urlItemList;
                string PostDataWithNoEencrypt = string.Format("{{\"RESULTKEY\":\"{0}\",{1}\"HEADER\":\"{2}\"}}"
                    , ResultKey,
                    string.IsNullOrWhiteSpace(model.RequestData) ? "" : model.RequestData + ",",
                    model.Header);
                var key = System.Text.Encoding.UTF8.GetBytes(model.SecureKey);
                var publicKey = System.Text.Encoding.UTF8.GetBytes(ResultKey);

                var buf = System.Text.Encoding.UTF8.GetBytes(PostDataWithNoEencrypt);
                var encryptBuf = ARC4Managed.Transform(buf, key);
                var postData = Convert.ToBase64String(encryptBuf);

                var sign = Convert.ToBase64String(SecurityManager.GetHash(SecurityManager.HashType.MD5, new StringBuilder().Append(PostDataWithNoEencrypt).Append(model.AppKey).ToString()));

                model.RequestData = string.Format("SIGN={0}&data={1}&APP_ID={2}", HttpUtility.UrlEncode(sign), HttpUtility.UrlEncode(postData), model.APP_ID);

                var apiResult = (new HttpClient()).PostGetApiObjectResult(model.RequestUrl, publicKey, model.RequestData, method: "Post");

                model.ResponseData = apiResult;
            }
            catch (Exception ex)
            {
                model.ResponseData = ex.Message;
            }


            return View(model);
        }
    }
}