using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;

namespace YeahTVApiLibrary.EntityFrameworkRepository.Repertory
{
    public class GlobalConfigRepertory : BaseRepertory<GlobalConfig, string>, IGlobalConfigRepertory
    {
        public override List<GlobalConfig> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as GlobalConfigCriteria;
            var query = base.Entities.AsQueryable();
            if (!string.IsNullOrEmpty(criteria.PermitionType))
            {
                query = query.Where(t => t.PermitionType == criteria.PermitionType).AsQueryable();
            }
            if (!string.IsNullOrEmpty(criteria.TypeId))
            {
                query = query.Where(t => t.TypeId == criteria.TypeId).AsQueryable();
            }
            return query.ToPageList(criteria);

        }
        public List<GlobalConfig> SearchByHotelId(string hotelId, string brandId, string groupId)
        {
            var query = from g in base.Entities
                        where
                            (g.TypeId == hotelId && g.PermitionType == "Hotel") ||
                            (g.TypeId == brandId && g.PermitionType == "Brand") ||
                            (g.TypeId == groupId && g.PermitionType == "Group")
                        select g;

            return query.ToList();

        }
        public List<GlobalConfig> SearchByBrandId(string brandId, string groupId)
        {
            var query = from g in base.Entities
                        where
                            (g.TypeId == brandId && g.PermitionType == "Brand") ||
                            (g.TypeId == groupId && g.PermitionType == "Group")
                        select g;

            return query.ToList();
        }
        public List<GlobalConfig> SearchByGroupId(string groupId)
        {
            var query = from g in base.Entities
                        where
                            (g.TypeId == groupId && g.PermitionType == "Group")
                        select g;

            return query.ToList();
        }
        public GlobalConfig GetGlobalConfig(GlobalConfig globalConfig)
        {
            var criteria = globalConfig;
            var query = base.Entities.AsQueryable();
            if (!string.IsNullOrEmpty(criteria.PermitionType))
            {
                query = query.Where(t => t.PermitionType == criteria.PermitionType).AsQueryable();
            }
            if (!string.IsNullOrEmpty(criteria.TypeId))
            {
                query = query.Where(t => t.TypeId == criteria.TypeId).AsQueryable();
            }
            if (!string.IsNullOrEmpty(criteria.Id))
            {
                query = query.Where(t => t.Id == criteria.Id).AsQueryable();
            }
            if (!string.IsNullOrEmpty(criteria.ConfigName))
            {
                query = query.Where(t => t.ConfigName == criteria.ConfigName);
            }
            return query.FirstOrDefault();
        }
    }
}
