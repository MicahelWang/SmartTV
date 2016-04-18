using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IMovieForLocalizeWrapperFacade
    { 
        List<MovieForLocalize> Search(MovieForLocalizeCriteria criteria);
        [Cache]
        List<MovieForLocalize> SearchMovieForLocalizes(MovieForLocalizeCriteria criteria);
        List<MovieForLocalize> SearchMovieForLocalizesByController(MovieForLocalizeCriteria criteria);
        MovieForLocalize FindByKey(string movieLocalizeId);
        [UnitOfWork]
        void AddMovie(MovieForLocalize movieLocalize);         
        [UnitOfWork]
        void UpdateWithLocalize(MovieForLocalize movieLocalize); 
        void Update(MovieForLocalize movieLocalize);
        [UnitOfWork]
        void DeleteMovie(MovieForLocalize movieLocalize);
    }
}
