using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Enum;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using System.Linq.Expressions;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.WrapperFacade
{
    public class HotelMovieTraceNoTemplateWrapperFacade : IHotelMovieTraceNoTemplateWrapperFacade
    {
        private readonly IHotelMovieTraceNoTemplateManager templateManager;
        private readonly IMovieForLocalizeManager movieForLocalizeManager;
        private readonly ITagManager tagManager;
        private readonly ILocalizeResourceManager sourceManager;
        private readonly ISysAttachmentManager sysAttachmentManager;
        private readonly IConstantSystemConfigManager constantSystemConfigManager;
        private readonly ITVHotelConfigManager tvHotelConfigManager;
        private readonly IDeviceTraceLibraryManager deviceTraceLibraryManager;
        private readonly IHCSTaskManager hcsTaskManager;
        private readonly IHCSGlobalConfigManager hcsGlobalConfigManager;
        public HotelMovieTraceNoTemplateWrapperFacade(IHotelMovieTraceNoTemplateManager _templateManager
            , IMovieForLocalizeManager _movieForLocalizeManager
            , ITagManager _tagManager
            , ILocalizeResourceManager _sourceManager
            , ISysAttachmentManager _sysAttachmentManager
            , IConstantSystemConfigManager _constantSystemConfigManager,
            ITVHotelConfigManager _tvHotelConfigManager,
            IDeviceTraceLibraryManager _deviceTraceLibraryManager,
            IHCSTaskManager _hcsTaskManager,
            IHCSGlobalConfigManager _hcsGlobalConfigManager)
        {
            templateManager = _templateManager;
            movieForLocalizeManager = _movieForLocalizeManager;
            tagManager = _tagManager;
            sourceManager = _sourceManager;
            sysAttachmentManager = _sysAttachmentManager;
            constantSystemConfigManager = _constantSystemConfigManager;
            tvHotelConfigManager = _tvHotelConfigManager;
            deviceTraceLibraryManager = _deviceTraceLibraryManager;
            hcsTaskManager = _hcsTaskManager;
            hcsGlobalConfigManager = _hcsGlobalConfigManager;
        }


        public List<HotelMovieTraceNoTemplate> SearchHotelMovieTraceNoTemplates(HotelMovieTraceNoTemplateCriteria criteria)
        {
            var list = templateManager.SearchWithLocalize(criteria);
            return GetHotelMovieTraceWithLocalizeList(list);
        }

        [Cache]
        public List<HotelMovieTraceNoTemplate> SearchHotelMovieTraceNoTemplatesFromCache(HotelMovieTraceNoTemplateCriteria criteria)
        {
            var list = templateManager.SearchWithLocalize(criteria);
            return GetHotelMovieTraceWithLocalizeList(list);
        }
        public List<HotelMovieTraceNoTemplate> GetHotelMovieTraceWithLocalizeList(List<HotelMovieTraceNoTemplate> list)
        {
            var tagList = tagManager.GetALLTagWithLocalizeResource();

            list.AsParallel().ForAll(t =>
                {
                    var tempList = new List<IEnumerable<LocalizeResource>>();
                    tagList.Where(o => t.MovieForLocalize.TagIds.Split(',').ToList().Contains(o.Id.ToString())).ToList()
                       .ForEach(n =>
                                {
                                    if (n.LocalizeResources.Count() > 0)
                                        tempList.Add(n.LocalizeResources);
                                });
                    t.MovieForLocalize.Tags = tempList;

                    var file = sysAttachmentManager.GetById(int.Parse(t.MovieForLocalize.CoverAddress ?? "0"));
                    if (file != null)
                        t.MovieForLocalize.CoverAddressPath = string.IsNullOrEmpty(t.MovieForLocalize.CoverAddress) ?
                    "" : constantSystemConfigManager.ResourceSiteAddress + file.FilePath;
                    if (!string.IsNullOrEmpty(t.MovieForLocalize.PosterAddress))
                        t.MovieForLocalize.PosterAddress.Split(',').ToList().ForEach(p =>
                        {
                            t.MovieForLocalize.PosterAddressPath = new List<string>();
                            var postFile = sysAttachmentManager.GetById(int.Parse(p));
                            if (postFile != null)
                                t.MovieForLocalize.PosterAddressPath.Add(constantSystemConfigManager.ResourceSiteAddress + postFile.FilePath);
                        });
                }
                );
            return list;
        }


        public void AddHotelMovieTraceNoTemplates(List<HotelMovieTraceNoTemplate> templates)
        {
            templateManager.AddHotelMovieTraceNoTemplates(templates);
        }

        public void AddHotelMovieTraceNoTemplate(HotelMovieTraceNoTemplate template)
        {
            templateManager.AddHotelMovieTraceNoTemplate(template);
        }

        public void Update(HotelMovieTraceNoTemplate template)
        {
            templateManager.Update(template);
        }

        public void Delete(HotelMovieTraceNoTemplate template)
        {
            templateManager.Delete(template);
        }


        public void DistributeByDevice(DeviceTrace device)
        {
            if (device.DeviceType.ToLower() != DeviceType.HCSServer.ToString().ToLower()) return;

            hcsGlobalConfigManager.AddServerTaskConfig(device.DeviceSeries);

            var templateMovies = templateManager.SearchHotelMovieTraceNoTemplates(new HotelMovieTraceNoTemplateCriteria { HotelId = device.HotelId });
            var movies = templateMovies.Where(m => !m.IsDelete).Select(m => m.MovieForLocalize).ToList();
            hcsTaskManager.AddMovieTaskByDevice(movies, device);
        }

        public void DistributeByHotel(CoreSysHotel hotel)
        {
            var movies = movieForLocalizeManager.Search(new MovieForLocalizeCriteria { DistributeAll = true });
            CheckHotelPaymentConfig(hotel.Id);

            var adds = new List<HotelMovieTraceNoTemplate>();

            movies.ForEach(movie =>
            {
                adds.Add(new HotelMovieTraceNoTemplate()
                {
                    HotelId = hotel.Id,
                    Active = true,
                    DownloadStatus = DownloadStatus.NotSynchronize.ToString(),
                    MovieId = movie.Id,
                    IsTop = movie.IsTop,
                    LastUpdateUser = "System",
                    LastViewTime = DateTime.Now,
                    Order = null,
                    Price = movie.DefaultAmount ?? 0m,
                    ViewCount = 0,
                    CreateTime = DateTime.Now
                });
            });

            if (adds.Count > 0)
            {
                var result = templateManager.RunTransaction(() =>
                {
                    var resultHotelMovies = templateManager.AddRangTransaction(adds);
                    var resultMovies = movieForLocalizeManager.ChangeHotelCount(movies, c => (c.HasValue) ? (int)(c = c.Value + 1) : 0);

                    return new Tuple<List<HotelMovieTraceNoTemplate>, List<MovieForLocalize>>(resultHotelMovies,
                        resultMovies);
                });
            }
        }

        public void Distribute(DistributeType distributeType, MovieForLocalize movie, List<CoreSysHotel> allHotels, string lastUpdateUser,
            ICollection<string> existHotels = null)
        {
            switch (distributeType)
            {
                case DistributeType.All:

                    RefreshHotelMovies(movie, allHotels, lastUpdateUser);
                    movie.HotelCount = allHotels.Count;
                    movie.DistributeAll = true;

                    break;
                case DistributeType.Cancel:

                    var templateMovies = templateManager.SearchHotelMovieTraceNoTemplates(new HotelMovieTraceNoTemplateCriteria { MovieId = movie.Id });
                    var hotelMovieTraceNoTemplates = templateMovies.Where(m => !m.IsDelete).Select(m => m.HotelId).ToList();

                    if (hotelMovieTraceNoTemplates.Count > 0)
                    {
                        templateManager.DeleteByMovieId(movie.Id);
                        movie.HotelCount = 0;
                        movie.DistributeAll = false;

                        var hotels = allHotels.Where(m => hotelMovieTraceNoTemplates.Contains(m.Id)).ToList();
                        if (hotels.Count > 0)
                            AddTask(hotels, movie, HCSJobOperationType.UnShelve);
                    }

                    break;
                case DistributeType.Part:

                    RefreshHotelMovies(movie, allHotels, lastUpdateUser, existHotels);
                    movie.HotelCount = allHotels.Count(m => existHotels != null && existHotels.Contains(m.Id));
                    movie.DistributeAll = false;

                    break;
            }
            movieForLocalizeManager.Update(movie);
        }

        public int Update(Expression<Func<HotelMovieTraceNoTemplate, bool>> Predicate, Expression<Func<HotelMovieTraceNoTemplate, HotelMovieTraceNoTemplate>> Updater)
        {
            int count = templateManager.Update(Predicate, Updater);
            return count;
        }

        private void RefreshHotelMovies(MovieForLocalize movie, List<CoreSysHotel> allHotels, string lastUpdateUser, ICollection<string> existHotels = null)
        {
            var hotels = allHotels;
            if (existHotels != null)
                hotels = hotels.Where(m => existHotels.Contains(m.Id)).ToList();

            var templateMovies = templateManager.SearchHotelMovieTraceNoTemplates(new HotelMovieTraceNoTemplateCriteria { MovieId = movie.Id });
            var hotelMovieTraceNoTemplates = templateMovies.ToList();

            var adds = new List<HotelMovieTraceNoTemplate>();
            var updates = new List<HotelMovieTraceNoTemplate>();


            var configCode = "HotelPayment";
            var hotelConfigs = tvHotelConfigManager.Search(new HotelConfigCriteria() { ConfigCodes = configCode.ToLower() }).ToList();

            hotels.ForEach(m =>
            {
                if (!hotelConfigs.Any(c => c.HotelId.Equals(m.Id)))
                    tvHotelConfigManager.AddHotelPaymentConfig(m.Id);

                HotelMovieTraceNoTemplate hotelMovieTrace;
                if ((hotelMovieTrace = hotelMovieTraceNoTemplates.FirstOrDefault(t => t.HotelId.Equals(m.Id))) == null)
                {
                    adds.Add(new HotelMovieTraceNoTemplate()
                    {
                        HotelId = m.Id,
                        Active = true,
                        DownloadStatus = DownloadStatus.NotSynchronize.ToString(),
                        MovieId = movie.Id,
                        IsTop = movie.IsTop,
                        LastUpdateUser = lastUpdateUser,
                        LastViewTime = DateTime.Now,
                        Order = null,
                        Price = movie.DefaultAmount ?? 0m,
                        ViewCount = 0,
                        CreateTime = DateTime.Now
                    });
                }
                else if (hotelMovieTrace.IsDelete)
                {
                    hotelMovieTrace.IsDelete = false;
                    updates.Add(hotelMovieTrace);
                }
            });


            if (adds.Count > 0)
                templateManager.AddHotelMovieTraceNoTemplates(adds);

            if (updates.Count > 0)
                templateManager.BatchChangeIsDelete(updates, false);

            if (existHotels != null)
            {
                var deletes = hotelMovieTraceNoTemplates.Where(m => !m.IsDelete).Where(m => !existHotels.Contains(m.HotelId)).ToList();
                if (deletes.Count > 0)
                {
                    templateManager.BatchChangeIsDelete(deletes, true);

                    var deleteHotelIds = deletes.Select(m => m.HotelId).ToList();
                    var deleteHotels = allHotels.Where(m => deleteHotelIds.Contains(m.Id)).ToList();
                    if (deleteHotels.Count > 0)
                        AddTask(deleteHotels, movie, HCSJobOperationType.UnShelve);
                }
            }
            AddTask(hotels, movie, HCSJobOperationType.Shelve);
        }


        private void AddTask(List<CoreSysHotel> hotels, MovieForLocalize movie, HCSJobOperationType operation)
        {
            var deviceTraces = deviceTraceLibraryManager.Search(new DeviceTraceCriteria { DeviceType = DeviceType.HCSServer }).ToList();
            hcsTaskManager.RestMovieTask(hotels, deviceTraces, movie, operation);
        }

        private void CheckHotelPaymentConfig(string hotelId)
        {
            var configCode = "HotelPayment";
            var hotelConfigs = tvHotelConfigManager.Search(new HotelConfigCriteria() { ConfigCodes = configCode.ToLower() }).ToList();

            if (!hotelConfigs.Any(c => c.HotelId.Equals(hotelId)))
                tvHotelConfigManager.AddHotelPaymentConfig(hotelId);
        }
    }
}
