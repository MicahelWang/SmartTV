using YeahTVApiLibrary.Manager;
namespace YeahTVApi.Manager
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel.Mapping;
    using System;
    using YeahTVApi.DomainModel.Models.DataModel;
    using YeahTVApiLibrary.EntityFrameworkRepository;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models.ViewModels;

    public class HotelMovieTraceManager : IHotelMovieTraceManager
    {
        private IHotelMovieTraceRepertory hotelMovieTraceRepertory;
        private IMovieTemplateRelationManager movieTemplateRelationManager;
        private ISysAttachmentManager sysAttachmentManager;
        private ITVHotelConfigManager tVHotelConfigManager;
        private IMovieTemplateManager movieTemplateManager;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private IRedisCacheService redisCacheService;
        private IRequestApiService requestApiService;
        public HotelMovieTraceManager(IHotelMovieTraceRepertory hotelMovieTraceRepertory,
            IMovieTemplateRelationManager movieTemplateRelationManager,
            ISysAttachmentManager sysAttachmentManager,
            ITVHotelConfigManager tVHotelConfigManager,
            IMovieTemplateManager movieTemplateManager,
            IConstantSystemConfigManager constantSystemConfigManager,

            IRedisCacheService redisCacheService,
            IRequestApiService requestApiService)
        {
            this.hotelMovieTraceRepertory = hotelMovieTraceRepertory;
            this.movieTemplateRelationManager = movieTemplateRelationManager;
            this.sysAttachmentManager = sysAttachmentManager;
            this.tVHotelConfigManager = tVHotelConfigManager;
            this.movieTemplateManager = movieTemplateManager;
            this.constantSystemConfigManager = constantSystemConfigManager;
            this.redisCacheService = redisCacheService;
            this.requestApiService = requestApiService;
        }


        #region Redis

        public List<HotelMovieTrace> GetAllFromCache()
        {
            return redisCacheService.GetAllFromCache(RedisKey.HotelMovieTracesKey, hotelMovieTraceRepertory.GetAllWithInclude);
        }

        public void RemoveCache()
        {
            redisCacheService.Remove(RedisKey.HotelMovieTracesKey);
        }

        #endregion

        public List<HotelMovieTrace> Search(HotelMovieTraceCriteria criteria)
        {
            try
            {
                return hotelMovieTraceRepertory.Search(criteria);
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("Search Error!", ex);
            }
        }

        public List<HotelMovieTraceViewModel> SearchForHotelTemplate(HotelMovieTraceCriteria searchCriteria)
        {
            try
            {
                return hotelMovieTraceRepertory.SearchForHotelTemplate(searchCriteria);
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("SearchForHotelTemplate Error!", ex);
            }
        }

        [UnitOfWork]
        public void AddMovieTraceManager(HotelMovieTrace trace)
        {
            try
            {
                var freshMovieTemplate = movieTemplateManager.GetAllFromCache().FirstOrDefault(m => m.Id == trace.MoiveTemplateId);
                freshMovieTemplate.HotelCount++;
                movieTemplateManager.Update(freshMovieTemplate);

                AddHotelMovieTrace(trace);
                //TODO:因使用事务无法使用缓存集合，所以先直接移除key来更新缓存
                RemoveCache();
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("AddMovieTraceManager Error!", ex);
            }
        }

        [UnitOfWork]
        public void UpdateMovieTraceManager(HotelMovieTrace trace)
        {
            try
            {
                var hotelMovieTemplateId = GetAllFromCache().Where(m => m.HotelId.Equals(trace.HotelId)).Select(s => s.MoiveTemplateId).FirstOrDefault();

                if (!hotelMovieTemplateId.Equals(trace.MoiveTemplateId))
                {
                    var nativeMovieTemplate = movieTemplateManager.GetAllFromCache().FirstOrDefault(m => m.Id == hotelMovieTemplateId);
                    nativeMovieTemplate.HotelCount--;
                    movieTemplateManager.Update(nativeMovieTemplate);

                    var freshMovieTemplate = movieTemplateManager.GetAllFromCache().FirstOrDefault(m => m.Id == trace.MoiveTemplateId);
                    freshMovieTemplate.HotelCount++;
                    movieTemplateManager.Update(freshMovieTemplate);

                    hotelMovieTraceRepertory.Delete(h => h.HotelId.Equals(trace.HotelId));
                    AddHotelMovieTrace(trace);
                }
                else //如果有匹配的模板，则更新模板电影
                {
                    var hotelMovies = GetAllFromCache().Where(m => m.HotelId.Equals(trace.HotelId) && m.MoiveTemplateId.Equals(trace.MoiveTemplateId)).ToList();
                    var modifyMovies = movieTemplateRelationManager.GetAllFromCache().Where(m => m.MovieTemplateId == trace.MoiveTemplateId).ToList();

                    //1,删除旧模板中没有的电影                    
                    List<string> hotelMovieIds = new List<string>(), hotelModifyMovieIds = new List<string>();
                    hotelMovies.Select(s => new { s.MovieId }).ToList().ForEach(m =>
                    {
                        hotelMovieIds.Add(m.MovieId);
                    });
                    modifyMovies.Select(m => new { m.MovieId }).ToList().ForEach(m =>
                    {
                        hotelModifyMovieIds.Add(m.MovieId);
                    });

                    var deleteHotelMovieids = hotelMovieIds.Except(hotelModifyMovieIds).ToList();
                    var deleteHotelMovies = hotelMovies.Where(m => deleteHotelMovieids.Any(n => n == m.MovieId)).ToList();
                    if (deleteHotelMovies.Count > 0)
                    {
                        var deleteMovieIds = deleteHotelMovies.Select(d => d.MovieId);
                        var deleteHotelIds = deleteHotelMovies.Select(d => d.HotelId);
                        hotelMovieTraceRepertory.Delete(m => deleteHotelIds.Contains(m.HotelId) && deleteMovieIds.Contains(m.MovieId));
                    }
                    //2,找到新模板中没有的电影，并插入movietrance表
                    var addMovieRelationids = hotelModifyMovieIds.Except(hotelMovieIds).ToList();
                    var addMovieRelations = modifyMovies.Where(s => addMovieRelationids.Contains(s.MovieId)).ToList();
                    if (addMovieRelations.Count > 0)
                    {
                        var hotelMovieTraces = new List<HotelMovieTrace>();
                        addMovieRelations.ForEach(m =>
                        {
                            var hotelMovieTrace = new HotelMovieTrace();
                            trace.CopyTo(hotelMovieTrace);
                            hotelMovieTrace.MovieId = m.MovieId;
                            hotelMovieTraces.Add(hotelMovieTrace);
                        });
                        hotelMovieTraceRepertory.Insert(hotelMovieTraces);
                    }
                }

                //TODO:因使用事务无法使用缓存集合，所以先直接移除key来更新缓存
                RemoveCache();

            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("UpdateMovieTraceManager Error!", ex);
            }
        }
        [Obsolete]
        public List<MovieApiModel> SearchMoviesForApi(RequestHeader header)
        {
            try
            {
                return GetMovieApiList(header);
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("SearchMoviesForApi Error!", ex);
            }
        }

        private List<MovieApiModel> GetMovieApiList(RequestHeader header, bool isOld = true)
        {
            var hotelMovieTraces = GetAllFromCache().Where(m => m.HotelId.Equals(header.HotelID) && m.IsDownload).OrderByDescending(h => h.Order);
            HotelEntity hotel = null;
            if (!isOld)
            {
                var requestHotelUrl = constantSystemConfigManager.AppCenterUrl + Constant.GetHotelApiUrl + header.HotelID;//"http://localhost:8088/"
                hotel = requestApiService.Get(requestHotelUrl).JsonStringToObj<HotelEntity>();
            }
            return hotelMovieTraces.Select(h => new MovieApiModel
            {
                CoverAddress = constantSystemConfigManager.ResourceSiteAddress + sysAttachmentManager.GetById(int.Parse(h.Movie.CoverAddress)).FilePath,
                IsFree = h.MovieTemplate.MovieTemplateRelations.Single(m => m.MovieId == h.MovieId).IsFree,
                LastViewTime = h.LastViewTime,
                MovieReview = h.Movie.MovieReview,
                MovieReviewEn = h.Movie.MovieReviewEn,
                Name = h.Movie.Name,
                NameEn = h.Movie.NameEn,
                PosterAddress = new List<string> { constantSystemConfigManager.ResourceSiteAddress + sysAttachmentManager.GetById(int.Parse(h.Movie.PosterAddress)).FilePath },
                Price = h.MovieTemplate.MovieTemplateRelations.Single(m => m.MovieId == h.MovieId).Price,
                ViewCount = h.ViewCount,
                MovieId = h.MovieId,
                VideoUrl = isOld ? tVHotelConfigManager.SearchFromCache(new HotelConfigCriteria {HotelId=h.HotelId }).First(m =>m.ConfigCode.Equals("VodAddress")).ConfigValue
                + "vod/" + h.MovieId + ".mp4" : hotel.VodAddress + "vod/" + h.MovieId + ".mp4"//tVHotelConfigManager.GetAllFromCache().First(m => m.HotelId.Equals(header.HotelID) && m.ConfigCode.Equals("VodAddress")).ConfigValue
                // + "vod/" + h.MovieId + ".mp4"

            }).ToList();
        }

        private void AddHotelMovieTrace(HotelMovieTrace trace)
        {
            var hotelMovieTraces = new List<HotelMovieTrace>();
            var movieInTemplates = movieTemplateRelationManager.GetAllFromCache().Where(m => m.MovieTemplateId == trace.MoiveTemplateId).ToList();

            movieInTemplates.ForEach(m =>
            {
                var hotelMovieTrace = new HotelMovieTrace();

                trace.CopyTo(hotelMovieTrace);

                hotelMovieTrace.MovieId = m.MovieId;
                hotelMovieTraces.Add(hotelMovieTrace);
            });
            hotelMovieTraceRepertory.Insert(hotelMovieTraces);
        }

        public bool UpdateMoviveTraceDownStatus(HotelMovieTrace trace)
        {
            HotelMovieTraceCriteria hotelMovieSingle=new HotelMovieTraceCriteria{
             HotelId=trace.HotelId,
             MovieId=trace.MovieId,
             MoiveTemplateId=trace.MoiveTemplateId
            };
            HotelMovieTrace hotelTraceSingle = hotelMovieTraceRepertory.Search(hotelMovieSingle).FirstOrDefault();
            try
            {
                hotelTraceSingle.IsDownload = trace.IsDownload;
                hotelMovieTraceRepertory.Update(hotelTraceSingle);
                RemoveCache();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
       
        public void UpdateMoviveTraceList(List<HotelMovieTrace> list, bool isDown)
        {
            list.ForEach(m => {
                m.IsDownload = isDown;
                hotelMovieTraceRepertory.Update(m);
            });
            RemoveCache();
        }
    }
}
