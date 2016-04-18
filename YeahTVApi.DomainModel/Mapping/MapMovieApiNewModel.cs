using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApi.DomainModel.Mapping
{
    public static class MapMovieApiNewModel
    {
        public static MovieApiNewModel ToMovieApiNewModel(this HotelMovieTraceNoTemplate hotelNoTem, string langType)
        {

            var movieList = new MovieApiNewModel()
            {
                Name = hotelNoTem.MovieForLocalize.Names.ToLocalizeResource(langType),
                MovieReview = hotelNoTem.MovieForLocalize.MovieReviews.ToLocalizeResource(langType),

                Language = hotelNoTem.MovieForLocalize.Languages.ToLocalizeResource(langType),
                Director = hotelNoTem.MovieForLocalize.Directors.ToLocalizeResource(langType),

                Tags = GetTags(hotelNoTem, langType),

                District = hotelNoTem.MovieForLocalize.Districts.ToLocalizeResource(langType),
                IsTop = hotelNoTem.MovieForLocalize.IsTop.HasValue ? hotelNoTem.MovieForLocalize.IsTop.Value : false,

                Mins = hotelNoTem.MovieForLocalize.Mins,
                Attribute = hotelNoTem.MovieForLocalize.Attribute,

                CoverAddress = string.IsNullOrEmpty(hotelNoTem.MovieForLocalize.CoverAddressPath) ? "" : hotelNoTem.MovieForLocalize.CoverAddressPath,
                PosterAddress = GetPostAddress(hotelNoTem),

                LastViewTime = hotelNoTem.LastViewTime,
                MovieId = hotelNoTem.MovieId,

                Price = hotelNoTem.Price,
                Rate = hotelNoTem.MovieForLocalize.Rate.HasValue ? hotelNoTem.MovieForLocalize.Rate.Value : 0,

                Starred = hotelNoTem.MovieForLocalize.Starreds.ToLocalizeResource(langType),

                ViewCount = Convert.ToInt32(hotelNoTem.ViewCount),
                Vintage = hotelNoTem.MovieForLocalize.Vintage.ToString(),
            };
            return movieList;
        }

        private static List<string> GetPostAddress(HotelMovieTraceNoTemplate hotelNoTem)
        {
            if (hotelNoTem.MovieForLocalize.PosterAddressPath != null)
                return hotelNoTem.MovieForLocalize.PosterAddressPath.ToList();
            else
                return null;
        }

        private static List<string> GetTags(HotelMovieTraceNoTemplate hotelNoTem, string langType)
        {
            List<IEnumerable<LocalizeResource>> list = hotelNoTem.MovieForLocalize.Tags.ToList();
            var strList = new List<string>();

            list.ForEach(t =>
            {
                if ((t.SingleOrDefault(m => m.Lang.ToUpper().Equals(langType.Trim().ToUpper()))) != null)
                {
                    if (!string.IsNullOrEmpty(t.SingleOrDefault(m => m.Lang.ToUpper().Equals(langType.ToUpper())).Content))
                        strList.Add(t.SingleOrDefault(m => m.Lang.ToUpper().Equals(langType.ToUpper())).Content);
                }
            });
            return strList;
        }

        public static string ToLocalizeResource(this IEnumerable<LocalizeResource> localizeResources, string langType)
        {
            return localizeResources.SingleOrDefault(N => N.Lang != null && N.Lang.ToUpper().Equals(langType.ToUpper())) == null ?
                "" : localizeResources.SingleOrDefault(N => N.Lang.ToUpper().Equals(langType.ToUpper())).Content;
        }
    }
}
