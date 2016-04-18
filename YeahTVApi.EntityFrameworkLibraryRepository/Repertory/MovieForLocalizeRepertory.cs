using System;
using System.Linq.Expressions;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using YeahTVApi.Common;
    using EntityFramework.Extensions;

    public class MovieForLocalizeRepertory : BaseRepertory<MovieForLocalize, string>, IMovieForLocalizeRepertory
    {

        public override List<MovieForLocalize> Search(BaseSearchCriteria searchCriteria)
        {
            var query = Query(searchCriteria);
            return query.ToPageList(searchCriteria);
        }

        private IQueryable<MovieForLocalize> Query(BaseSearchCriteria searchCriteria)
        {
            var deviceSearchCriteria = searchCriteria as MovieForLocalizeCriteria;
            var query = base.Entities.AsQueryable();

            if (!string.IsNullOrEmpty(deviceSearchCriteria.Id))
            {
                query = query.Where(q => q.Id.Equals(deviceSearchCriteria.Id));
            }
            if (!string.IsNullOrEmpty(deviceSearchCriteria.Name))
            {
                query = query.Where(q => q.Name.Equals(deviceSearchCriteria.Name));
            }
            if (!string.IsNullOrEmpty(deviceSearchCriteria.TagId))
            {
                query = query.Where(q => !string.IsNullOrEmpty(q.TagIds) && ("," + q.TagIds + ",").Contains("," + deviceSearchCriteria.TagId + ","));
            }
            if (deviceSearchCriteria.DistributeAll.HasValue)
            {
                query = query.Where(q => q.DistributeAll==(deviceSearchCriteria.DistributeAll.Value));
            }
            if (deviceSearchCriteria.HotelCount != -1)
            {
                if (deviceSearchCriteria.HotelCount == 0)
                {
                    query = query.Where(q => q.HotelCount == 0);
                }
                else if (deviceSearchCriteria.HotelCount == 1)
                {
                    query = query.Where(q => q.HotelCount > 0);
                }
            }
            if (deviceSearchCriteria.IsTop)
            {
                query = query.Where(q => q.IsTop == true);
            }
            return query;
        }
        public List<MovieForLocalize> SearchMoiveWithLocalize(BaseSearchCriteria searchCriteria)
        {
            
            //("," + movie.TagIds + ",")
            var query = Query(searchCriteria).ToPageQueryable(searchCriteria);
            var queryfromDB = from movie in query
                              join r in base.Context.Set<LocalizeResource>() on movie.Name equals r.Id into gName
                              from resouceName in gName.DefaultIfEmpty()
                              join d in base.Context.Set<LocalizeResource>() on movie.Director equals d.Id into gDirector
                              from resouceDirector in gDirector.DefaultIfEmpty() 
                              join s in  base.Context.Set<LocalizeResource>() on movie.Starred equals s.Id into gStarred
                              from resouceStarred in gStarred.DefaultIfEmpty()
                              join Dis in base.Context.Set<LocalizeResource>() on movie.District equals Dis.Id into gDistrict
                              from resouceDistrict in gDistrict.DefaultIfEmpty()
                              join m in base.Context.Set<LocalizeResource>() on movie.MovieReview equals m.Id into gMovieReview
                              from resouceMovieReview in gMovieReview.DefaultIfEmpty()
                              join l in base.Context.Set<LocalizeResource>() on movie.Language equals l.Id into gLanguage
                              from resouceLanguage in gLanguage.DefaultIfEmpty()
                              select new
                              {
                                  Id = movie.Id,
                                  Name = movie.Name,
                                  NameId = resouceName.Id,
                                  NameContent = resouceName.Content,
                                  NameLang = resouceName.Lang,

                                  Director = movie.Director,
                                  DirectorId = resouceDirector.Id,
                                  DirectorContent = resouceDirector.Content,
                                  DirectorLang = resouceDirector.Lang,

                                  Starred = movie.Starred,
                                  StarredId = resouceStarred.Id,
                                  StarredContent = resouceStarred.Content,
                                  StarredLang = resouceStarred.Lang,

                                  District = movie.District,
                                  DistrictId = resouceDistrict.Id,
                                  DistrictContent = resouceDistrict.Content,
                                  DistrictLang = resouceDistrict.Lang,

                                  MovieReview = movie.MovieReview,
                                  MovieReviewId = resouceMovieReview.Id,
                                  MovieReviewContent = resouceMovieReview.Content,
                                  MovieReviewLang = resouceMovieReview.Lang,

                                  Language = movie.Language,
                                  LanguageId = resouceLanguage.Id,
                                  LanguageContent = resouceLanguage.Content,
                                  LanguageLang = resouceLanguage.Lang,

                                  PosterAddress = movie.PosterAddress,
                                  CoverAddress = movie.CoverAddress,

                                  Mins = movie.Mins,
                                  Vintage = movie.Vintage,
                                  TagIds = movie.TagIds,
                                  VodUrl = movie.VodUrl,
                                  MD5 = movie.MD5,
                                  Rate = movie.Rate,
                                  DefaultAmount = movie.DefaultAmount,
                                  CurrencyType = movie.CurrencyType,
                                  Attribute = movie.Attribute,
                                  Order = movie.Order,
                                  IsTop = movie.IsTop,
                                  CreateTime = movie.CreateTime,
                                  DistributeAll = movie.DistributeAll,
                                  FirstWord = movie.FirstWord,
                                  LastUpdateTime = movie.LastUpdateTime,
                                  LastUpdateUser = movie.LastUpdateUser,
                                  HotelCount = movie.HotelCount
                              };
            var test = from item in
                           queryfromDB.ToList()
                       group item by item.Id into g
                       select g;
            var result = (from item in
                              queryfromDB.ToList()
                          group item by item.Id into g
                          select new MovieForLocalize
                          {
                              Id = g.FirstOrDefault().Id,
                              Name = g.FirstOrDefault().Name,
                              Names = g.Select(s => new LocalizeResource
                              {
                                  Content = s.NameContent,
                                  Id = s.NameId,
                                  Lang = s.NameLang
                              }).Distinct(new LocalizeResourceDistinct()),

                              Director = g.FirstOrDefault().Director,
                              Directors = g.Select(s => new LocalizeResource
                              {
                                  Content = s.DirectorContent,
                                  Id = s.DirectorId,
                                  Lang = s.DirectorLang
                              }).Distinct(new LocalizeResourceDistinct()),

                              Starred = g.FirstOrDefault().Starred,
                              Starreds = g.Select(s => new LocalizeResource
                              {
                                  Content = s.StarredContent,
                                  Id = s.StarredId,
                                  Lang = s.StarredLang
                              }).Distinct(new LocalizeResourceDistinct()),

                              District = g.FirstOrDefault().District,
                              Districts = g.Select(s => new LocalizeResource
                              {
                                  Content = s.DistrictContent,
                                  Id = s.DistrictId,
                                  Lang = s.DistrictLang
                              }).Distinct(new LocalizeResourceDistinct()),

                              MovieReview = g.FirstOrDefault().MovieReview,
                              MovieReviews = g.Select(s => new LocalizeResource
                              {
                                  Content = s.MovieReviewContent,
                                  Id = s.MovieReviewId,
                                  Lang = s.MovieReviewLang
                              }).Distinct(new LocalizeResourceDistinct()),

                              Language = g.FirstOrDefault().Language,
                              Languages = g.Select(s => new LocalizeResource
                              {
                                  Content = s.LanguageContent,
                                  Id = s.LanguageId,
                                  Lang = s.LanguageLang
                              }).Distinct(new LocalizeResourceDistinct()),

                              PosterAddress = g.FirstOrDefault().PosterAddress,
                              CoverAddress = g.FirstOrDefault().CoverAddress,

                              Mins = g.FirstOrDefault().Mins,
                              Vintage = g.FirstOrDefault().Vintage,
                              TagIds = g.FirstOrDefault().TagIds,
                              VodUrl = g.FirstOrDefault().VodUrl,
                              MD5 = g.FirstOrDefault().MD5,
                              Rate = g.FirstOrDefault().Rate,
                              DefaultAmount = g.FirstOrDefault().DefaultAmount,
                              CurrencyType = g.FirstOrDefault().CurrencyType,
                              Attribute = g.FirstOrDefault().Attribute,
                              Order = g.FirstOrDefault().Order,
                              IsTop = g.FirstOrDefault().IsTop,
                              CreateTime = g.FirstOrDefault().CreateTime,
                              DistributeAll = g.FirstOrDefault().DistributeAll,
                              FirstWord = g.FirstOrDefault().FirstWord,
                              LastUpdateTime = g.FirstOrDefault().LastUpdateTime,
                              LastUpdateUser = g.FirstOrDefault().LastUpdateUser,
                              HotelCount = g.FirstOrDefault().HotelCount
                          }
                         ).ToList();

            return result;
        }

        public void ChangeHotelCount(Expression<Func<MovieForLocalize, bool>> filterExpression, Func<int?, int> changeFun)
        {
            base.Entities.Where(filterExpression).ToList().ForEach(m =>
            {
                m.HotelCount = changeFun(m.HotelCount);
                Update(m);
            });
        }
        

    }

    public class LocalizeResourceDistinct : IEqualityComparer<LocalizeResource>
    {
        public bool Equals(LocalizeResource x, LocalizeResource y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.Id == y.Id && x.Lang == y.Lang;
        }

        public int GetHashCode(LocalizeResource moive)
        {
            if (Object.ReferenceEquals(moive, null)) return 0;
            int hashStudentName = moive.Id == null ? 0 : moive.Id.GetHashCode();
            int hashStudentCode = moive.Lang == null ? 0 : moive.Lang.GetHashCode();
            return hashStudentName ^ hashStudentCode;
        }
    }
}
