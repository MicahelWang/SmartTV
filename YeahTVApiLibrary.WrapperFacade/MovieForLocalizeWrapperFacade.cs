using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.Enum;
namespace YeahTVApiLibrary.WrapperFacade
{
    public class MovieForLocalizeWrapperFacade : IMovieForLocalizeWrapperFacade
    {
        private readonly IHotelMovieTraceNoTemplateManager templateManager;
        private readonly IMovieForLocalizeManager movieForLocalizeManager;
        private readonly ITagManager tagManager;
        private readonly ILocalizeResourceManager sourceManager;
        private readonly ISysAttachmentManager sysAttachmentManager;
        private readonly IConstantSystemConfigManager constantSystemConfigManager;
        public MovieForLocalizeWrapperFacade(IHotelMovieTraceNoTemplateManager _templateManager
            , IMovieForLocalizeManager _movieForLocalizeManager
            , ITagManager _tagManager
            , ILocalizeResourceManager _sourceManager
            , ISysAttachmentManager _sysAttachmentManager
            , IConstantSystemConfigManager _constantSystemConfigManager)
        {
            templateManager = _templateManager;
            movieForLocalizeManager = _movieForLocalizeManager;
            tagManager = _tagManager;
            sourceManager = _sourceManager;
            sysAttachmentManager = _sysAttachmentManager;
            constantSystemConfigManager = _constantSystemConfigManager;
        }


        public List<MovieForLocalize> SearchMovieForLocalizes(MovieForLocalizeCriteria criteria)
        {
            var list = movieForLocalizeManager.SearchMovieForLocalizes(criteria);
            return GetMovieWithLocalizeList(list);
        }
        public List<MovieForLocalize> SearchMovieForLocalizesByController(MovieForLocalizeCriteria criteria)
        {
            var list = movieForLocalizeManager.SearchMovieForLocalizes(criteria);
            return GetMovieWithLocalizeList(list);
        }
        private List<MovieForLocalize> GetMovieWithLocalizeList(List<MovieForLocalize> list)
        {
            var tagList = tagManager.GetALLTagWithLocalizeResource();

            list.AsParallel().ForAll(t =>
                {

                    var tagTemp = new List<IEnumerable<LocalizeResource>>();
                    tagList.Where(o => !string.IsNullOrEmpty(t.TagIds) && ("," + t.TagIds + ",").Contains("," + o.Id + ",")).ToList()
                       .ForEach(n =>
                            {
                                tagTemp.Add(n.LocalizeResources);
                            });
                    t.Tags = tagTemp;
                    var sourceAddress = constantSystemConfigManager.ResourceSiteAddress;
                    if (!string.IsNullOrEmpty(t.CoverAddress) && !string.IsNullOrEmpty(sourceAddress))
                    {
                        var file = sysAttachmentManager.GetById(int.Parse(t.CoverAddress.Replace(" ", "")));
                        if (file != null)
                            t.CoverAddressPath = string.IsNullOrEmpty(t.CoverAddress) ?
                            "" : sourceAddress + file.FilePath; ;
                    }
                    if (!string.IsNullOrEmpty(t.PosterAddress) && !string.IsNullOrEmpty(sourceAddress))
                    {
                        var postTemp = new List<string>();
                        t.PosterAddress.Split(',').ToList().ForEach(p =>
                        {
                            var postFile = sysAttachmentManager.GetById(int.Parse(p));
                            if (postFile != null)
                                postTemp.Add(sourceAddress + postFile.FilePath);
                        });
                        t.PosterAddressPath = postTemp;
                    }
                }
                );
            return list;
        }

        #region Movie Add

        public void AddMovie(MovieForLocalize model)
        {
            var localize = GetLocalListByMovie(model);
            sourceManager.AddLocalizeResources(localize);
            movieForLocalizeManager.Add(model);
            //分发在contrler中处理
        }

        private static List<LocalizeResource> GetLocalListByMovie(MovieForLocalize model)
        {
            var localize = new List<LocalizeResource>();
            localize.AddRange(model.Names);
            localize.AddRange(model.Languages);
            localize.AddRange(model.Directors);
            localize.AddRange(model.MovieReviews);
            localize.AddRange(model.Districts);
            localize.AddRange(model.Starreds);
            return localize;
        }
        #endregion

        #region Movie Update
        public void Update(MovieForLocalize movie)
        {
            movieForLocalizeManager.Update(movie);
        }
        
        public void UpdateWithLocalize(MovieForLocalize movie)
        {
            var oldModel = FindByKey(movie.Id);
            var localize = GetLocalListByMovie(movie);
            var oldLocalize = GetLocalListByMovie(oldModel);
            sourceManager.Delete(oldLocalize.Select(m => m.Id).Distinct().ToArray());
            sourceManager.AddLocalizeResources(localize);
            //分发 在controller中处理 
            movieForLocalizeManager.Update(movie);
        }
        #endregion

        #region Movie Delete
         
        public void DeleteMovie(MovieForLocalize model)
        {
            var movie = FindByKey(model.Id);
            var resource = new List<string>();
            resource.Add(movie.Name);
            resource.Add(movie.Language);
            resource.Add(movie.Director);
            resource.Add(movie.MovieReview);
            resource.Add(movie.District);
            resource.Add(movie.Starred);
            sourceManager.Delete(resource.ToArray());
            templateManager.DeleteByMovieId(movie.Id);
            movieForLocalizeManager.Delete(movie);
        }
        #endregion

        public MovieForLocalize FindByKey(string movieLocalizeId)
        {
            var movie = movieForLocalizeManager.FindByKey(movieLocalizeId);
            if (movie != null)
                return GetMovieWithLocalizeList(new List<MovieForLocalize>() { movie }).First();
            return movie;
        }

        public List<MovieForLocalize> Search(MovieForLocalizeCriteria criteria)
        {
            return movieForLocalizeManager.Search(criteria);
        }
    }
}
