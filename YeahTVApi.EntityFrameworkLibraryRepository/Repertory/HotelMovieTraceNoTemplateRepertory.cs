using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApiLibrary.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.DomainModel;
    using EntityFramework.Extensions;
    using YeahTVApi.DomainModel.Enum;

    public class HotelMovieTraceNoTemplateRepertory : BaseRepertory<HotelMovieTraceNoTemplate, string>, IHotelMovieTraceNoTemplateRepertory
    {

        public List<HotelMovieTraceNoTemplate> SearchWithLocalize(BaseSearchCriteria searchCriteria)
        {
            if (searchCriteria.SortFiled.Equals("Id"))
                searchCriteria.SortFiled = "HotelId";

            var query = Query(searchCriteria).ToPageQueryable(searchCriteria);

            var queryfromDB = from hotelMovie in query
                              join r in base.Context.Set<LocalizeResource>() on hotelMovie.MovieForLocalize.Name equals r.Id into gName
                              from resouceName in gName.DefaultIfEmpty()
                              join d in base.Context.Set<LocalizeResource>() on hotelMovie.MovieForLocalize.Director equals d.Id into gDirector
                              from resouceDirector in gDirector.DefaultIfEmpty()
                              join s in base.Context.Set<LocalizeResource>() on hotelMovie.MovieForLocalize.Starred equals s.Id into gStarred
                              from resouceStarred in gStarred.DefaultIfEmpty()
                              join Dis in base.Context.Set<LocalizeResource>() on hotelMovie.MovieForLocalize.District equals Dis.Id into gDistrict
                              from resouceDistrict in gDistrict.DefaultIfEmpty()
                              join m in base.Context.Set<LocalizeResource>() on hotelMovie.MovieForLocalize.MovieReview equals m.Id into gMovieReview
                              from resouceMovieReview in gMovieReview.DefaultIfEmpty()
                              join l in base.Context.Set<LocalizeResource>() on hotelMovie.MovieForLocalize.Language equals l.Id into gLanguage
                              from resouceLanguage in gLanguage.DefaultIfEmpty()
                              select new
                              {
                                  HotelMovieInfo = hotelMovie,
                                  HotelId = hotelMovie.HotelId,
                                  MovieId = hotelMovie.MovieId,
                                  Name = hotelMovie.MovieForLocalize.Name,
                                  NameId = resouceName.Id,
                                  NameContent = resouceName.Content,
                                  NameLang = resouceName.Lang,

                                  Director = hotelMovie.MovieForLocalize.Director,
                                  DirectorId = resouceDirector.Id,
                                  DirectorContent = resouceDirector.Content,
                                  DirectorLang = resouceDirector.Lang,

                                  Starred = hotelMovie.MovieForLocalize.Starred,
                                  StarredId = resouceStarred.Id,
                                  StarredContent = resouceStarred.Content,
                                  StarredLang = resouceStarred.Lang,

                                  District = hotelMovie.MovieForLocalize.District,
                                  DistrictId = resouceDistrict.Id,
                                  DistrictContent = resouceDistrict.Content,
                                  DistrictLang = resouceDistrict.Lang,

                                  MovieReview = hotelMovie.MovieForLocalize.MovieReview,
                                  MovieReviewId = resouceMovieReview.Id,
                                  MovieReviewContent = resouceMovieReview.Content,
                                  MovieReviewLang = resouceMovieReview.Lang,

                                  Language = hotelMovie.MovieForLocalize.Language,
                                  LanguageId = resouceLanguage.Id,
                                  LanguageContent = resouceLanguage.Content,
                                  LanguageLang = resouceLanguage.Lang,

                                  PosterAddress = hotelMovie.MovieForLocalize.PosterAddress,
                                  CoverAddress = hotelMovie.MovieForLocalize.CoverAddress,

                                  Mins = hotelMovie.MovieForLocalize.Mins,
                                  Vintage = hotelMovie.MovieForLocalize.Vintage,
                                  TagIds = hotelMovie.MovieForLocalize.TagIds,
                                  VodUrl = hotelMovie.MovieForLocalize.VodUrl,
                                  MD5 = hotelMovie.MovieForLocalize.MD5,
                                  Rate = hotelMovie.MovieForLocalize.Rate,
                                  DefaultAmount = hotelMovie.MovieForLocalize.DefaultAmount,
                                  CurrencyType = hotelMovie.MovieForLocalize.CurrencyType,
                                  Attribute = hotelMovie.MovieForLocalize.Attribute,
                                  Order = hotelMovie.MovieForLocalize.Order,
                                  IsTop = hotelMovie.MovieForLocalize.IsTop,
                                  CreateTime = hotelMovie.MovieForLocalize.CreateTime,
                                  DistributeAll = hotelMovie.MovieForLocalize.DistributeAll,
                                  FirstWord = hotelMovie.MovieForLocalize.FirstWord,
                                  LastUpdateTime = hotelMovie.MovieForLocalize.LastUpdateTime,
                                  LastUpdateUser = hotelMovie.MovieForLocalize.LastUpdateUser,
                                  HotelCount = hotelMovie.MovieForLocalize.HotelCount
                              };

            var result = (from item in
                              queryfromDB.ToList()
                          group item by new { item.HotelId, item.MovieId } into g
                          select new HotelMovieTraceNoTemplate
                          {
                              Active = g.FirstOrDefault().HotelMovieInfo.Active,
                              CreateTime = g.FirstOrDefault().HotelMovieInfo.CreateTime,
                              DownloadStatus = g.FirstOrDefault().HotelMovieInfo.DownloadStatus,
                              HotelId = g.FirstOrDefault().HotelId,
                              MovieId = g.FirstOrDefault().MovieId,
                              IsDelete = g.FirstOrDefault().HotelMovieInfo.IsDelete,
                              IsTop = g.FirstOrDefault().HotelMovieInfo.IsTop,
                              LastUpdateUser = g.FirstOrDefault().HotelMovieInfo.LastUpdateUser,
                              LastViewTime = g.FirstOrDefault().HotelMovieInfo.LastViewTime,
                              Order = g.FirstOrDefault().HotelMovieInfo.Order,
                              Price = g.FirstOrDefault().HotelMovieInfo.Price,
                              ViewCount = g.FirstOrDefault().HotelMovieInfo.ViewCount,
                              MovieForLocalize = new MovieForLocalize()
                              {
                                  Id = g.FirstOrDefault().MovieId,
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
                                  CoverAddressPath = g.FirstOrDefault().CoverAddress,

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
                          }
                         );

            return result.ToList();
        }

        private IQueryable<HotelMovieTraceNoTemplate> Query(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as HotelMovieTraceNoTemplateCriteria;

            var query = base.Entities.Include("MovieForLocalize").AsQueryable();

            if (!string.IsNullOrEmpty(criteria.HotelId))
                query = query.Where(q => q.HotelId.Equals(criteria.HotelId));

            if (!string.IsNullOrEmpty(criteria.MovieId))
                query = query.Where(q => q.MovieId.Equals(criteria.MovieId));

            if (!string.IsNullOrEmpty(criteria.DownloadStatus))
                query = query.Where(q => q.DownloadStatus.Equals(criteria.DownloadStatus));

            if (!string.IsNullOrEmpty(criteria.CategoryId))
            {
                query = query.Where(q => !string.IsNullOrEmpty(q.MovieForLocalize.TagIds) && ("," + q.MovieForLocalize.TagIds + ",").Contains("," + criteria.CategoryId + ","));
            }

            if (criteria.Active.HasValue)
                query = query.Where(q => q.Active.Equals(criteria.Active.Value));

            if (criteria.IsDelete.HasValue)
                query = query.Where(q => q.IsDelete.Equals(criteria.IsDelete.Value));


            return query;
        }
        public override List<HotelMovieTraceNoTemplate> Search(BaseSearchCriteria searchCriteria)
        {
            if (searchCriteria.SortFiled.Equals("Id"))
                searchCriteria.SortFiled = "HotelId";
            var query = Query(searchCriteria);
            return query.ToPageList(searchCriteria);
        }

        public new List<HotelMovieTraceNoTemplate> GetAll()
        {
            var query = base.Entities.Include("MovieForLocalize").AsQueryable();
            return query.ToList();
        }
        public void BatchChangeIsDelete(Expression<Func<HotelMovieTraceNoTemplate, bool>> filterExpression, bool isDelete)
        {
            Entities.Where(filterExpression).ToList().ForEach(m =>
            {
                m.IsDelete = isDelete;

                Update(m);
            });
        }
    }
}
