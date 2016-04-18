using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;
using YeahAppCentre.Web.Utility;
using YeahCenter.Infrastructure;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class DashBoardController : BaseController
    {
        private readonly IMongoDeviceTraceManager _mongoDeviceTraceManager;
        private readonly IVODOrderManager _vodOrderManager;
        private readonly IBehaviorLogManager _behaviorLogManager;
        private readonly IDeviceTraceLibraryManager _deviceTraceLibraryManager;
        private readonly IBackupDeviceManager _backupDeviceManager;
        private readonly IHotelPermitionManager _hotelPermitionManager;
        private readonly IDashBoardManager _dashBoardManager;
        private readonly IHotelManager _hotelManager;
        private readonly IMovieForLocalizeWrapperFacade _movieManager;
        private readonly ITVChannelManager _tvChannelManager;
        private readonly IBrandManager _brandManager;
        private readonly IConstantSystemConfigManager _constantSystemConfigManager;

        public DashBoardController(IMongoDeviceTraceManager mongoDeviceTraceManager
            , IVODOrderManager vodOrderManager
            , IMovieForLocalizeWrapperFacade movieManager
            , ITVChannelManager tvChannelManager
            , IBehaviorLogManager behaviorLogManager
            , IDeviceTraceLibraryManager deviceTraceLibraryManager
            , IBackupDeviceManager backupDeviceManager
            , IHotelPermitionManager hotelPermitionManager
            , IDashBoardManager dashBoardManager
            , IHotelManager hotelManager
            , IBrandManager brandManager
            , IConstantSystemConfigManager constantSystemConfigManager)
        {
            _mongoDeviceTraceManager = mongoDeviceTraceManager;
            _vodOrderManager = vodOrderManager;
            _behaviorLogManager = behaviorLogManager;
            _movieManager = movieManager;
            _tvChannelManager = tvChannelManager;
            this._deviceTraceLibraryManager = deviceTraceLibraryManager;
            this._backupDeviceManager = backupDeviceManager;
            _hotelPermitionManager = hotelPermitionManager;
            _dashBoardManager = dashBoardManager;
            _hotelManager = hotelManager;
            _brandManager = brandManager;
            _constantSystemConfigManager = constantSystemConfigManager;

        }

        public ActionResult VodOrderReportIndex(VODOrderCriteria vodOrderCriteria)
        {
            vodOrderCriteria.NeedPaging = true;

            var partialViewResult = this.VodOrderReportList(vodOrderCriteria) as PartialViewResult;
            if (partialViewResult != null)
            {
                this.ViewBag.List = partialViewResult.Model;
            }

            ViewBag.VODOrderCriteria = vodOrderCriteria;

            return View();
        }

        [AjaxOnly]
        public ActionResult VodOrderReportList(VODOrderCriteria vodOrderReportCriteria)
        {
            var list = new PagedViewList<VODOrder>();

            if (vodOrderReportCriteria.SortFiled.Equals("Id"))
            {
                vodOrderReportCriteria.SortFiled = "CompleteTime";
                vodOrderReportCriteria.OrderAsc = false;
            }

            if (!string.IsNullOrWhiteSpace(vodOrderReportCriteria.CompleteTimeRange))
            {
                var dateArray = vodOrderReportCriteria.CompleteTimeRange.Split('-');
                vodOrderReportCriteria.CompleteBeginTime = Convert.ToDateTime(dateArray[0].Trim());
                vodOrderReportCriteria.CompleteEndTime = Convert.ToDateTime(string.Format("{0} 23:59:59", dateArray[1].Trim()));
            }

            // 设置初始条件
            vodOrderReportCriteria.orderState = OrderState.Success;

            vodOrderReportCriteria.NeedPaging = true;
            list.PageIndex = vodOrderReportCriteria.Page;
            list.PageSize = vodOrderReportCriteria.PageSize;
            list.Source = _vodOrderManager.SearchOrders(vodOrderReportCriteria);
            list.TotalCount = vodOrderReportCriteria.TotalCount;

            // 设置显示格式
            foreach (VODOrder current in list.Source)
            {
                current.PayInfo = current.PayInfo.ParseAsEnum<PayPaymentModel>().ConvertToInt().ToString().GetEnumDescription<PayPaymentModel>();
            }

            return this.PartialView("VodOrderReportList", list);
        }

        public ActionResult ModifyInvoiceTitle(string id, OpType type, string hotelId)
        {
            VODOrder order = new VODOrder();
            VODOrderCriteria vodOrderReportCriteria;

            ViewBag.OpType = type;
            switch (type)
            {
                case OpType.Add:
                    break;
                case OpType.View:
                case OpType.Update:

                    vodOrderReportCriteria = new VODOrderCriteria();
                    vodOrderReportCriteria.OrderId = id;

                    order = _vodOrderManager.SearchOrders(vodOrderReportCriteria).FirstOrDefault();
                    break;
                default:
                    break;
            }

            return PartialView(order);
        }

        [HttpPost]
        public ActionResult ModifyInvoiceTitle(VODOrder viewOrder, OpType type)
        {
            string errorMsg = string.Empty;
            try
            {
                switch (type)
                {
                    case OpType.Add:
                        break;

                    case OpType.Update:
                        VODOrderCriteria vodOrderReportCriteria = new VODOrderCriteria();
                        vodOrderReportCriteria.OrderId = viewOrder.Id;

                        VODOrder order = _vodOrderManager.SearchOrders(vodOrderReportCriteria).FirstOrDefault();
                        order.InvoicesTitle = viewOrder.InvoicesTitle;
                        _vodOrderManager.UpdateVODOrder(order);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            return this.Content(string.IsNullOrEmpty(errorMsg) ? "Success" : errorMsg);
        }

        // GET: DashBoard
        public ActionResult Index(string hotelId)
        {
            var hotelInfoStatistics = new HotelInfoStatistics
            {
                HotelId = hotelId,
                CoreSysHotel = _hotelManager.GetCoreSysHotelById(hotelId)
            };

            if (hotelInfoStatistics.CoreSysHotel == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var deviceStatistics = _dashBoardManager.GetStatisticsHotelList(new List<CoreSysHotel>() { hotelInfoStatistics.CoreSysHotel });

            if (hotelInfoStatistics.CoreSysHotel == null || deviceStatistics == null || deviceStatistics.Count == 0)
            {
                return RedirectToAction("Index", "Account");
            }

            hotelInfoStatistics.CoreSysBrand = _brandManager.GetBrand(hotelInfoStatistics.CoreSysHotel.BrandId);
            hotelInfoStatistics.CoreSysBrand.Logo = _constantSystemConfigManager.ResourceSiteAddress + hotelInfoStatistics.CoreSysBrand.Logo;

            var hotelDeviceTotal = deviceStatistics.First();

            hotelInfoStatistics.ValidityDays = _constantSystemConfigManager.DashBoardValidityDays;
            hotelInfoStatistics.BackUpDeviceSeriesCount = hotelDeviceTotal.BackUpDeviceSeriesCount;
            hotelInfoStatistics.DeviceTraceSeriesCount = hotelDeviceTotal.DeviceTraceSeriesCount;
            hotelInfoStatistics.DeviceSeriesTotal = hotelDeviceTotal.DeviceSeriesTotal;

            var yesterday = DateTime.Now.AddDays(-1).Date;
            var yesterdayEnd = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 59);

            var hotelStartPercentage = _mongoDeviceTraceManager.GetHotelStartPercentage(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria()
                {
                    VisitTimeBegin = yesterday,
                    VisitTimeEnd = yesterdayEnd,
                    HotelId = hotelId
                }).FirstOrDefault();
            hotelInfoStatistics.YesterdayActive = hotelStartPercentage != null ? hotelStartPercentage.ActiveCount : 0;

            var hotelMovieDailyIncome = _vodOrderManager.GetHotelMovieDailyIncome(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria()
                {
                    VisitTimeBegin = yesterday,
                    VisitTimeEnd = yesterdayEnd,
                    HotelId = hotelId
                }).FirstOrDefault();

            hotelInfoStatistics.YesterdayMovieIncome = hotelMovieDailyIncome != null ? hotelMovieDailyIncome.Income : 0;


            var hotelModuleUsedTime = _behaviorLogManager.GetHotelModuleUsedTime(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria()
                {
                    VisitTimeBegin = yesterday,
                    VisitTimeEnd = yesterdayEnd,
                    HotelId = hotelId
                });

            hotelInfoStatistics.YesterdayUsedTime = hotelModuleUsedTime != null ? hotelModuleUsedTime.Sum(m => m.Value) : 0;


            var listTotal = _hotelPermitionManager.GetHotelListByPermition(CurrentUser.UID);
            ViewBag.MoreHotel = (listTotal.Count > 1);

            return View(hotelInfoStatistics);
        }

        #region dashboard 单酒店
        [AjaxOnly]
        public JsonResult GetHotelStartPercentage(string hotelId, DateTime? beginDate, DateTime? endDate)
        {
            if (!beginDate.HasValue || !endDate.HasValue)
            {
                beginDate = DateTime.Now.AddDays(-8);
                endDate = DateTime.Now.AddDays(-1);
            }
            else if (endDate.Value.Date == DateTime.Now.Date)
            {
                endDate = endDate.Value.AddDays(-1);
            }
            beginDate = new DateTime(beginDate.Value.Year, beginDate.Value.Month, beginDate.Value.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);

            var hotelStartPercentage = _mongoDeviceTraceManager.GetHotelStartPercentage(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria() { VisitTimeBegin = beginDate, VisitTimeEnd = endDate, HotelId = hotelId }).OrderBy(m => m.Date);
            var seriesTitle = "开机率";
            var chartItems = new ChartItems<double>();
            chartItems.SeriesList.Add(new Series<double>()
            {
                Data = hotelStartPercentage.Select(m => m.StartPercentageOfDay).ToList(),
                Id = 1,
                Name = seriesTitle,
                Type = "line"
            });

            chartItems.XAxisList.AddRange(hotelStartPercentage.Select(m => m.Date.ToString("MM-dd")).ToList());

            chartItems.LegendList.Add(seriesTitle);

            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult GetHotelMovieDailyIncome(string hotelId, DateTime? beginDate, DateTime? endDate)
        {
            if (!beginDate.HasValue || !endDate.HasValue)
            {
                beginDate = DateTime.Now.Date.AddDays(-30);
                endDate = DateTime.Now.Date.AddDays(-1);
            }
            else if (endDate.Value.Date == DateTime.Now.Date)
            {
                endDate = endDate.Value.AddDays(-1).Date;
            }
            beginDate = new DateTime(beginDate.Value.Year, beginDate.Value.Month, beginDate.Value.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);

            var hotelMovieDailyIncome = _vodOrderManager.GetHotelMovieDailyIncome(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria() { VisitTimeBegin = beginDate.Value, VisitTimeEnd = endDate.Value, HotelId = hotelId }).OrderBy(m => m.Date);
            var seriesTitle = "收益";
            var chartItems = new ChartItems<decimal>();
            chartItems.SeriesList.Add(new Series<decimal>()
            {
                Data = hotelMovieDailyIncome.Select(m => m.Income).ToList(),
                Id = 1,
                Name = seriesTitle,
                Type = "line"
            });

            chartItems.XAxisList.AddRange(hotelMovieDailyIncome.Select(m => m.Date.ToString("MM-dd")).ToList());

            chartItems.LegendList.Add(seriesTitle);

            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult GetHotelMovieIncomeRanking(string hotelId, DateTime? beginDate, DateTime? endDate)
        {
            if (!beginDate.HasValue || !endDate.HasValue)
            {
                beginDate = DateTime.Now.Date.AddMonths(-6);
                endDate = DateTime.Now.Date.AddDays(-1);
            }
            else if (endDate.Value.Date == DateTime.Now.Date)
            {
                endDate = endDate.Value.AddDays(-1).Date;
            }
            beginDate = new DateTime(beginDate.Value.Year, beginDate.Value.Month, beginDate.Value.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);

            var hotelMovieIncomeRankings = _vodOrderManager.GetHotelMovieIncomeRanking(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria() { VisitTimeBegin = beginDate, VisitTimeEnd = endDate, HotelId = hotelId }).OrderBy(m => m.Income);
            var seriesTitle = "收益";
            var chartItems = new ChartItems<decimal>();
            chartItems.SeriesList.Add(new Series<decimal>()
            {
                Data = hotelMovieIncomeRankings.Select(m => m.Income).ToList(),
                Id = 1,
                Name = seriesTitle,
                Type = "line"
            });

            //TODO:现有读取电影名称方法太慢，先使用替换法处理名称
            chartItems.XAxisList.AddRange(hotelMovieIncomeRankings.Select(m => m.MovieName.Replace("单片点播-", "")).ToList());

            chartItems.LegendList.Add(seriesTitle);

            chartItems.SubTitle = string.Format("{0}~{1}", beginDate.Value.ToString("yyyy-MM-dd"), endDate.Value.ToString("yyyy-MM-dd"));

            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }


        [AjaxOnly]
        public JsonResult GetHotelModuleUsedTime(string hotelId, DateTime? beginDate, DateTime? endDate)
        {
            if (!beginDate.HasValue || !endDate.HasValue)
            {
                beginDate = DateTime.Now.AddDays(-8);
                endDate = DateTime.Now.AddDays(-1);
            }
            else if (endDate.Value.Date == DateTime.Now.Date)
            {
                endDate = endDate.Value.AddDays(-1);
            }

            beginDate = new DateTime(beginDate.Value.Year, beginDate.Value.Month, beginDate.Value.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);

            var hotelModuleUsedTime = _behaviorLogManager.GetHotelModuleUsedTime(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria() { VisitTimeBegin = beginDate, VisitTimeEnd = endDate, HotelId = hotelId }).OrderBy(m => m.Value);
            var seriesTitle = "使用占比/时长";
            var chartItems = new ChartItems<PieItem>();

            string path = Server.MapPath("~/Content/SiteXml/Constant.xml");

            var xElement = System.Xml.Linq.XElement.Load(path)
                .Element("ModuleConfigs");
            if (xElement != null)
            {
                var modules = xElement
                    .Elements("option")
                    .Select(m => new { Module = m.Value, DisplayName = m.Attribute("name").Value })
                    .ToList();

                Func<string, string> getDisplayName = moduleName =>
                {
                    var moduleInfo = modules.FirstOrDefault(module => module.Module.ToLower().Equals(moduleName.ToLower()));
                    return moduleInfo != null ? moduleInfo.DisplayName : moduleName;
                };

                chartItems.SeriesList.Add(new Series<PieItem>()
                {
                    Data = hotelModuleUsedTime.Select(m => new PieItem() { Value = m.Value, Name = getDisplayName(m.Key.ToString()) }).ToList(),
                    Id = 1,
                    Name = seriesTitle,
                    Type = "pie"
                });

                chartItems.XAxisList.AddRange(hotelModuleUsedTime.Select(m => getDisplayName(m.Key.ToString())).ToList());
                chartItems.LegendList.AddRange(hotelModuleUsedTime.Select(m => getDisplayName(m.Key.ToString())).ToList());
            }


            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }


        [AjaxOnly]
        public JsonResult GetHotelChannelUsedTime(string hotelId, DateTime? beginDate, DateTime? endDate)
        {
            if (!beginDate.HasValue || !endDate.HasValue)
            {
                beginDate = DateTime.Now.AddDays(-8);
                endDate = DateTime.Now.AddDays(-1);
            }
            else if (endDate.Value.Date == DateTime.Now.Date)
            {
                endDate = endDate.Value.AddDays(-1);
            }

            beginDate = new DateTime(beginDate.Value.Year, beginDate.Value.Month, beginDate.Value.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);

            var hotelModuleUsedTime = _behaviorLogManager.GetHotelChannelUsedTime(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria() { VisitTimeBegin = beginDate, VisitTimeEnd = endDate, HotelId = hotelId }).OrderByDescending(m => m.Value);
            var seriesTitle = "时长";
            var chartItems = new ChartItems<double>();
            chartItems.SeriesList.Add(new Series<double>()
            {
                Data = hotelModuleUsedTime.Select(m => m.Value).ToList(),
                Id = 1,
                Name = seriesTitle,
                Type = "pie"
            });


            var keys = hotelModuleUsedTime.Select(m => m.Key).ToList();
            var channels = _tvChannelManager.SearchChannelsByIds(keys);

            chartItems.XAxisList.AddRange(keys.Select(k =>
            {
                var channel = channels.FirstOrDefault(m => m.Id.Equals(k));
                return channel != null ? channel.Name : k;
            }));

            chartItems.LegendList.AddRange(hotelModuleUsedTime.Select(m => m.Key.ToString()).ToList());

            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult GetHotelMovieVodOfWeek(string hotelId, DateTime? beginDate)
        {
            if (!beginDate.HasValue)
            {
                beginDate = DateTime.Now;
            }

            beginDate = beginDate.Value.AddDays(-(int)beginDate.Value.DayOfWeek);
            DateTime? endDate = beginDate.Value.AddDays(6);


            beginDate = new DateTime(beginDate.Value.Year, beginDate.Value.Month, beginDate.Value.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);


            var hotelModuleUsedTime = _behaviorLogManager.GetHotelMovieVodOfDay(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria() { VisitTimeBegin = beginDate, VisitTimeEnd = endDate, HotelId = hotelId }).OrderBy(m => m.Value);
            var seriesTitle = "点播次数";
            var chartItems = new ChartItems<double>();
            chartItems.SeriesList.Add(new Series<double>()
            {
                Data = hotelModuleUsedTime.Select(m => m.Value).ToList(),
                Id = 1,
                Name = seriesTitle,
                Type = "line"
            });

            var keys = hotelModuleUsedTime.Select(m => m.Key).ToList();

            chartItems.XAxisList.AddRange(keys.Select(GetMovieNameZhCn));

            chartItems.LegendList.AddRange(hotelModuleUsedTime.Select(m => m.Key.ToString()).ToList());

            chartItems.Title = string.Format("视频点播{0}年第{1}周排行", beginDate.Value.Year, (new GregorianCalendar()).GetWeekOfYear(beginDate.Value, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday));
            chartItems.SubTitle = string.Format("{0}~{1}", beginDate.Value.ToString("yyyy-MM-dd"), endDate.Value.ToString("yyyy-MM-dd"));

            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }
        [AjaxOnly]
        public JsonResult GetHotelMovieVod(string hotelId)
        {
            var hotelMovieVodOfDay = _behaviorLogManager.GetHotelMovieVodOfDay(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria() { HotelId = hotelId }).OrderBy(m => m.Value);
            var seriesTitle = "点播次数";
            var chartItems = new ChartItems<double>();
            chartItems.SeriesList.Add(new Series<double>()
            {
                Data = hotelMovieVodOfDay.Select(m => m.Value).ToList(),
                Id = 1,
                Name = seriesTitle,
                Type = "line"
            });
            var keys = hotelMovieVodOfDay.Select(m => m.Key).ToList();

            chartItems.XAxisList.AddRange(keys.Select(GetMovieNameZhCn));

            chartItems.LegendList.AddRange(hotelMovieVodOfDay.Select(m => m.Key.ToString()).ToList());

            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }

        private string GetMovieNameZhCn(string movieId)
        {
            var result = "";
            var movie = _movieManager.FindByKey(movieId);
            if (movie == null) return result;
            var movieNames = movie.Names.FirstOrDefault(n => n.Lang.ToLower() == "zh-cn");
            if (movieNames != null)
                result = movieNames.Content;
            return result;
        }

        #endregion


        #region dashboard 多酒店

        public ActionResult List(CoreSysHotelCriteria seachCriteria)
        {
            var listTotal = _hotelPermitionManager.GetHotelListByPermition(CurrentUser.UID);
            var statisticsAllHotelList = _dashBoardManager.GetStatisticsHotelList(listTotal);

            if (listTotal.Count == 1)
            {
                return RedirectToAction("Index", new { hotelId = listTotal.First().Id });
            }

            ViewBag.BrandConfigUrls = _hotelPermitionManager.GetBrandConfigUrlsByUserId(CurrentUser.UID);

            ViewBag.DeviceSeriesTotal = statisticsAllHotelList.Sum(m => m.DeviceSeriesTotal);
            ViewBag.HotelListByPermition = statisticsAllHotelList.Count;

            var statisticsHotelList = statisticsAllHotelList.AsQueryable();
            if (!string.IsNullOrEmpty(seachCriteria.Id))
                statisticsHotelList = statisticsHotelList.Where(m => m.HotelId == seachCriteria.Id);

            var list = new PagedViewList<HotelInfoStatistics>();
            seachCriteria.NeedPaging = true;
            list.PageIndex = seachCriteria.Page;
            list.PageSize = seachCriteria.PageSize;
            list.Source = statisticsHotelList.AsQueryable().Page(seachCriteria.PageSize, seachCriteria.Page).ToList();
            list.TotalCount = statisticsHotelList.Count();
            return this.PartialView("MoreHotelList", list);
        }
        public ActionResult MoreHotelIndex(string HotelId, string HotelName)
        {
            var seachCriteria = new CoreSysHotelCriteria() { Id = HotelId };
            var result = List(seachCriteria);

            if (result is RedirectToRouteResult)
            {
                return result;
            }

            var partialViewResult = result as PartialViewResult;
            if (partialViewResult != null)
            {
                ViewBag.List = partialViewResult.Model;
            }

            ViewBag.ValidityDays = _constantSystemConfigManager.DashBoardValidityDays;
            ViewBag.HotelCriteria = seachCriteria;
            ViewBag.HotelId = HotelId;
            ViewBag.HotelName = HotelName;
            return View();
        }

        [AjaxOnly]
        public JsonResult QueryMoreHotelMovieVod()
        {
            DateTime? beginDate = null;
            DateTime? endDate = null;
            if (!beginDate.HasValue || !endDate.HasValue)
            {
                beginDate = DateTime.Now.AddDays(-8);
                endDate = DateTime.Now.AddDays(-1);
            }
            else if (endDate.Value.Date == DateTime.Now.Date)
            {
                endDate = endDate.Value.AddDays(-1);
            }

            beginDate = new DateTime(beginDate.Value.Year, beginDate.Value.Month, beginDate.Value.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);

            var hotelList = _hotelPermitionManager.GetHotelListByPermition(CurrentUser.UID).Select(m => m.Id).Distinct().ToList();
            var hotelModuleUsedTime = _behaviorLogManager.GetHotelMovieVodOfDay(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria()
                {
                    VisitTimeBegin = beginDate,
                    VisitTimeEnd = endDate,
                    HotelList = hotelList
                });
            var seriesTitle = "点播次数";
            var chartItems = new ChartItems<double>();
            chartItems.SeriesList.Add(new Series<double>()
            {
                Data = hotelModuleUsedTime.Select(m => m.Value).ToList(),
                Id = 1,
                Name = seriesTitle,
                Type = "line"
            });

            chartItems.XAxisList.AddRange(hotelModuleUsedTime.Select(m => m.Key.ToString()).ToList());

            chartItems.LegendList.AddRange(hotelModuleUsedTime.Select(m => m.Key.ToString()).ToList());

            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }

        [AjaxOnly]
        public JsonResult GetMoreHotelMovieIncomeRanking(DateTime beginDate, DateTime endDate)
        {
            beginDate = new DateTime(beginDate.Year, beginDate.Month, beginDate.Day, 0, 0, 0);
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);

            var hotelList = _hotelPermitionManager.GetHotelListByPermition(CurrentUser.UID);
            var hotelIdList = hotelList.Select(m => m.Id).Distinct().ToList();
            var hotelMovieIncomeRankings = _vodOrderManager.GetHotelsMovieIncomeRanking(
                new YeahTVApi.DomainModel.SearchCriteria.DashboardCriteria() { VisitTimeBegin = beginDate, VisitTimeEnd = endDate, HotelList = hotelIdList });
            var seriesTitle = "收益";
            var chartItems = new ChartItems<decimal>();
            chartItems.SeriesList.Add(new Series<decimal>()
            {
                Data = hotelMovieIncomeRankings.Select(m => m.Value).ToList(),
                Id = 1,
                Name = seriesTitle,
                Type = "line"
            });

            chartItems.XAxisList.AddRange(hotelMovieIncomeRankings.Select(m => hotelList.First(h => h.Id == m.Key).HotelName).ToList());

            chartItems.LegendList.Add(seriesTitle);

            return Json(chartItems, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region IUHotelConfig IU酒店 个性化配置
        public ActionResult IUConfigIndex(string brandID)
        {
            ViewBag.brandID = brandID;
            return View();
        }
        public ActionResult IUStartMuisc()
        {
            return View();
        }
        public ActionResult IUWelcomeBg()
        {
            return View();
        }
        public ActionResult IUIndexBG()
        {
            return View();
        }
        public ActionResult IUWelcomeWord()
        {
            return View();
        }
        public ActionResult IUPicCarousel()
        {
            return View();
        }
        public ActionResult IULauncher()
        {
            return View();
        }
        public ActionResult IUIndexBgMuisc()
        {
            return View();
        }
        public ActionResult IUStartVideo()
        {
            return View();
        }
        #endregion
    }
}