namespace YeahTVApi.Controllers
{
    using System.Collections.Generic;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.Models.ViewModels;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Controllers;
    using YeahTVApiLibrary.Filter;
    using YeahTVApiLibrary.Infrastructure;
    using YeahTVApi.DomainModel.Mapping;
    using YeahTVApi.DomainModel.Models.DataModel;
    using System.Linq;
    using System;
    using YeahAppCentre.Web.Utility;
    using YeahTVApi.DomainModel.Enum;
    using YeahTVApi.DomainModel.Models.DomainModels;
    using YeahTVApi.DomainModel.SearchCriteria;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class MovieTVChanelsResourcesController : BaseController
    {
        private IHotelTVChannelManager hotelTVChannelManager;
        private IHotelMovieTraceManager hotelMovieTraceManager;
        private IRedisCacheManager redisCacheManager;
        private IRequestApiService requestApiService;
        private IHttpContextService httpContextService;
        private ILogManager logManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IHotelMovieTraceNoTemplateWrapperFacade hotelMovieWrapperFacade;
        private ITagManager tagManager;
        private ITVHotelConfigManager tvHotelConfigManager;

        public MovieTVChanelsResourcesController(
            IHotelTVChannelManager hotelTVChannelManager,
            IHotelMovieTraceManager hotelMovieTraceManager,
            IRedisCacheManager redisCacheManager,
            IRequestApiService requestApiService,
            IHttpContextService httpContextService,
            ILogManager logManager,
            IConstantSystemConfigManager constantSystemConfigManager,
            IHotelMovieTraceNoTemplateWrapperFacade hotelMovieWrapperFacade,
            ITagManager tagManager, ITVHotelConfigManager tvHotelConfigManager)
        {
            this.hotelTVChannelManager = hotelTVChannelManager;
            this.hotelMovieTraceManager = hotelMovieTraceManager;
            this.redisCacheManager = redisCacheManager;
            this.requestApiService = requestApiService;
            this.httpContextService = httpContextService;
            this.logManager = logManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.hotelMovieWrapperFacade = hotelMovieWrapperFacade;
            base.HttpContextService = httpContextService;
            this.tagManager = tagManager;
            this.tvHotelConfigManager = tvHotelConfigManager;
        }

        [HTApiFilter]
        public ApiObjectResult<object> GetHotelChanels()
        {
            var channelList = GetTVChannelByHeader();
            return new ApiObjectResult<object> { obj = channelList };
        }


        [HTApiFilter]
        //public ApiObjectResult<List<MovieApiModel>> GetHotelMovies()
        public ApiObjectResult<object> GetHotelMovies()
        {
            var movies = GetHotelMoviesByHeader();
            return new ApiObjectResult<object> { obj = movies };
            // return new ApiObjectResult<List<MovieApiModel>> { obj = movies };
        }

        #region 获取电视频道模板接口

        [HTApiFilter]
        public ApiObjectResult<object> GetTemplateListModel()
        {
            var result = new ApiObjectResult<object>();
            var TvTemplate = new TemplateTVList();
            TvTemplate.objectInfo = GetTVChannelByHeader();
            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;
            var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();

            var templateJsonString = GetVodTemplate(TemplateRootType.TV);
            if (JObject.Parse(templateJsonString).GetValue("elements") != null)
                TvTemplate.TemplateContent = JObject.Parse(templateJsonString).GetValue("elements").First();
            //else
            //{
            //    object resultObj = GetSystemDefaultTVParameter();
            //    if (resultObj != null)
            //        TvTemplate.TemplateContent = resultObj;
            //    else
            //        return result.WithError("系统配置表中没有默认的配置信息");

            //}
            return new ApiObjectResult<object> { obj = TvTemplate };

        }

        #endregion

        #region 获取电影详细列表带Vod节点

        [HTApiFilter]
        public ApiObjectResult<object> GetTemplateMovieAndPrice(string langType, string pageIndex, string categoryType)
        {
            var result = new ApiObjectResult<object>();
            var vodTemplate = new TemplateVOD();
            string langTrimType = langType.Trim();
            vodTemplate.objectInfo = GetHotelMoviesByNewHeader(langTrimType, pageIndex, categoryType);
            if (vodTemplate.Categories.Count == 0)
                vodTemplate.Categories = null;
            return new ApiObjectResult<object> { obj = vodTemplate };
        }


        #endregion

        #region 获取电影分类目录接口

        [HTApiFilter]
        public ApiObjectResult<object> GetCategoryAndTemplate(string langType)
        {
            var result = new ApiObjectResult<object>();
            var vodTemplate = new TemplateVOD();
            string langTrimType = langType.Trim();
            HotelPayment hotelPayment = null;

            var templateJsonString = GetVodTemplate(TemplateRootType.VOD);

            //if (!templateJsonString.Equals("{}"))
            if (JObject.Parse(templateJsonString).GetValue("elements") != null)
                vodTemplate.TemplateContent = JObject.Parse(templateJsonString).GetValue("elements").First();
            else
            {
                object resultObj = GetSystemDefaultVodBackGround();
                if (resultObj != null)
                    vodTemplate.TemplateContent = resultObj;
                else
                    return result.WithError("系统配置表中没有默认的VOD配置信息");

            }
            var hotelPaymentInfo = tvHotelConfigManager.SearchFromCache(new HotelConfigCriteria() {HotelId=Header.HotelID })
            .FirstOrDefault(
             m => m.ConfigCode.ToLower().Equals("HotelPayment".ToLower()));

            if (hotelPaymentInfo == null)
                return result.WithError("未找到支付配置信息！");

            var configs = new string[] { "\"PriceOfDay\"", "\"PaymentModels\"", "\"PayType\"" };
            if (!configs.All(c => hotelPaymentInfo.ConfigValue.ToLower().Contains(c.ToLower())))
                return result.WithError("支付配置信息异常！");
            try
            {
                hotelPayment = JsonConvert.DeserializeObject<HotelPayment>(hotelPaymentInfo.ConfigValue);
            }
            catch (Exception e)
            {
                return result.WithError("支付配置信息异常！");
            }
            vodTemplate.HotelPayment = hotelPayment;

            var tags = tagManager.GetALLTagWithLocalizeResource();

            tags.AsParallel().ForAll(t =>
            {
                var categoryName = t.LocalizeResources.Where(l => l.Lang.ToUpper().Trim().Equals(langType.ToUpper().Trim())).FirstOrDefault();
                vodTemplate.Categories.Add(new MovieApiCategoryModel
                {
                    Id = t.Id.ToString(),
                    Name = categoryName == null ? "" : categoryName.Content,
                    IconUrl = string.IsNullOrEmpty(t.Icon)
                    ? "" : constantSystemConfigManager.ResourceSiteAddress
                    + t.Icon
                });
            });

            vodTemplate.Categories = vodTemplate.Categories.OrderBy(c => c.Id.ToInt()).ToList();

            return new ApiObjectResult<object> { obj = vodTemplate };
        }

        #endregion
        private Dictionary<string, object> GetSystemDefaultVodBackGround()
        {

            var template = new Dictionary<string, object>();
            try
            {
                template.Add("VodBackground", constantSystemConfigManager.VodBackground);
                template.Add("VodColor", constantSystemConfigManager.VodColor);
                template.Add("Name", "VOD");
                return template;
            }
            catch (Exception e)
            {
                return null;
            }


        }

        [Obsolete]
        private List<MovieApiModel> GetHotelMoviesByHeader()
        {
            var movies = new List<MovieApiModel>();
            try
            {
                movies = hotelMovieTraceManager.SearchMoviesForApi(Header);
            }
            catch (Exception ex)
            {
                logManager.SaveError(ex, ex.InnerException, DomainModel.Enum.AppType.CommonFramework);
            }
            return movies;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LangType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="categoryType"></param>
        /// <returns></returns>
        private List<MovieApiNewModel> GetHotelMoviesByNewHeader(string LangType, string pageIndex, string categoryType)
        {
            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;
            var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();
            var movieNewList = new List<MovieApiNewModel>();
            try
            {
                var resultMovieList = new List<HotelMovieTraceNoTemplate>();

                var hotelMovieTraceNoTemplateCriteria = new HotelMovieTraceNoTemplateCriteria
                {
                    CategoryId = categoryType,
                    NeedPaging = true,
                    PageSize = 8,
                    Page = int.Parse(pageIndex),
                    HotelId = Header.HotelID,
                    SortFiled = "IsTop,ViewCount,CreateTime",
                    DownloadStatus=DownloadStatus.Success.ToString(),
                    Active=true,
                    IsDelete=false
                };

                if (hotelMovieTraceNoTemplateCriteria.CategoryId == "2")
                {
                    hotelMovieTraceNoTemplateCriteria.CategoryId = "";
                    hotelMovieTraceNoTemplateCriteria.SortFiled = "CreateTime";
                }
                resultMovieList = GetMovieDetail(hotelMovieTraceNoTemplateCriteria);
                foreach (HotelMovieTraceNoTemplate hotelNoTem in resultMovieList)
                {
                    movieNewList.Add(hotelNoTem.ToMovieApiNewModel(LangType));
                };
                movieNewList.ForEach(l =>
                {
                    l.VodLocalUrl = string.IsNullOrEmpty(hotel.VodAddress)
                        ? "" : hotel.VodAddress + "/vod/" + l.MovieId + ".mp4";
                });
                return movieNewList;
            }
            catch (Exception ex)
            {
                logManager.SaveError(ex, ex.InnerException, DomainModel.Enum.AppType.CommonFramework);
                return null;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryType"></param>
        /// <param name="movieList"></param>
        private List<HotelMovieTraceNoTemplate> GetMovieDetail(HotelMovieTraceNoTemplateCriteria hotelMovieTraceNoTemplateCriteria)
        {
            var movieList = new List<HotelMovieTraceNoTemplate>();

            movieList = hotelMovieWrapperFacade.SearchHotelMovieTraceNoTemplatesFromCache(hotelMovieTraceNoTemplateCriteria);

            return movieList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private Tag GetSingleTag(string t)
        {
            Tag tagSingle = new Tag();
            tagSingle = tagManager.GetALLTagWithLocalizeResource().SingleOrDefault(l => l.Id.Equals(Convert.ToInt32(t)));             
            return tagSingle;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetVodTemplate(TemplateRootType templateType)//
        {
            var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;//"http://localhost:8088/"
            var hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();

            var templateJsonString = UtilityHelper.RequestTemplateByRootName(
            hotel.TemplateId, constantSystemConfigManager.AppCenterUrl + Constant.GetTemplateUrl, templateType)
            .ToString();
            return templateJsonString;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="langType"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        private static string GetCategoryName(string langType, Tag tag)
        {
            return tag.LocalizeResources.SingleOrDefault(l => l.Lang.ToUpper().Equals(langType.ToUpper())) == null ? "" :
                                tag.LocalizeResources.Where(l => l.Lang.ToUpper().Equals(langType.ToUpper())).FirstOrDefault().Content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private List<HotelTVChannelApiModel> GetTVChannelByHeader()
        {
            var list = new List<HotelTVChannelApiModel>();
            var hotelJsonString = string.Empty;
            var requestHotelUrl = string.Empty;

            try
            {
                var tvChannels = hotelTVChannelManager.SearchHotelTVChannels(Header);

                if (!tvChannels.Any())
                {
                    throw new ApiException("不能查询对应的酒店信息");
                }

                requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + Header.HotelID;

                var hotel = new HotelEntity();

                logManager.SaveInfo("GetHotelChanels", requestHotelUrl, DomainModel.Enum.AppType.CommonFramework);

                hotelJsonString = requestApiService.Get(requestHotelUrl);

                logManager.SaveInfo("GetHotelChanels", hotelJsonString, DomainModel.Enum.AppType.CommonFramework);

                hotel = hotelJsonString.JsonStringToObj<HotelEntity>();

                logManager.SaveInfo("GetHotelChanels", hotel, DomainModel.Enum.AppType.CommonFramework);


                list = tvChannels.ToHotelTvChannelApiModel(hotel.AdUrl)
                    .OrderBy(t => t.ChannelOrder).ToList();
            }
            catch (Exception ex)
            {
                logManager.SaveError(ex, "GetHotelChanels: " + requestHotelUrl + "------------------------------------" + hotelJsonString + "------------------------------------" + ex.InnerException, DomainModel.Enum.AppType.CommonFramework);
            }


            return list;
        }


    }
}