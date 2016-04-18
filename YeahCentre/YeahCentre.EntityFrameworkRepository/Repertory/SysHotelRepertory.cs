using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Infrastructure;
using YeahTVApiLibrary.EntityFrameworkRepository.Repertory;

namespace YeahCentre.EntityFrameworkRepository.Repertory
{
    public class SysHotelRepertory : BaseRepertory<CoreSysHotel, string>, ISysHotelRepertory
    {

        public HotelEntity GetHotelEntity(string hotelId)
        {
            var hotel = GetQueryable().SingleOrDefault(m => m.HotelId.Equals(hotelId));
            return hotel;
        }

        public List<HotelEntity> GetHotelEntityByBrand(string brandId)
        {

            var hotelArray = GetQueryable().ToList();
            return hotelArray;

        }

        public List<HotelEntity> GetHotelEntity()
        {
            var hotelArray = GetQueryable().ToList();
            return hotelArray;

        }

        private IQueryable<HotelEntity> GetQueryable()
        {
            var query = Entities.Include("CoreSysHotelSencond").AsQueryable();
            var queryable = query.Where(m => !m.IsDelete).Select(q => new HotelEntity()
            {
                HotelId = q.Id,
                HotelCode = q.HotelCode,
                BrandId = q.BrandId,
                GroupId = q.GroupId,
                HotelName = q.HotelName,
                HotelNameEn = q.HotelNameEn,
                Province = q.Province,
                City = q.City,
                Address = q.Address,
                Longitude = q.Longitude,
                Latitude = q.Latitude,
                Country = q.Country,
                Tel = q.Tel,
                TemplateId = q.TemplateId,
                IsLocalPms = q.IsLocalPMS,
                AutoToHome = q.CoreSysHotelSencond.AutoToHome,
                Languages = q.CoreSysHotelSencond.Languages,
                WelcomeWord = q.CoreSysHotelSencond.WelcomeWord,
                LaunchBackground = q.CoreSysHotelSencond.LaunchBackground,
                LocalPmsUrl = q.CoreSysHotelSencond.LocalPMSUrl,
                PriceOfDay = q.CoreSysHotelSencond.PriceOfDay,
                AdUrl = q.CoreSysHotelSencond.AdUrl,
                BaseData = q.CoreSysHotelSencond.BaseData,
                LogoImageUrl = q.CoreSysHotelSencond.LogoImageUrl,
                VodAddress = q.CoreSysHotelSencond.VodAddress
            });
            return queryable;
        }

        public new List<CoreSysHotel> GetAll()
        {
           return this.Entities
               .Include("CoreSysHotelSencond")
               .Include("CoreSysBrand.CoreSysGroup")
               .Include("TvTemplate").Where(h => !h.IsDelete).ToList();
        }

        public override List<CoreSysHotel> Search(BaseSearchCriteria searchCriteria)
        {
            var criteria = searchCriteria as CoreSysHotelCriteria;

            var query = this.Entities.Include("CoreSysHotelSencond").Include("CoreSysBrand.CoreSysGroup").Include("TvTemplate").Where(h => !h.IsDelete).AsQueryable();

            if (!string.IsNullOrEmpty(criteria.Id))
                query = query.Where(q => q.Id.Equals(criteria.Id));

            if (!string.IsNullOrEmpty(criteria.BrandId))
                query = query.Where(q => q.BrandId.Equals(criteria.BrandId));

            if (!string.IsNullOrEmpty(criteria.GroupId))
                query = query.Where(q => q.GroupId.Equals(criteria.GroupId));

            if (!string.IsNullOrEmpty(criteria.HotelName))
                query = query.Where(q => q.HotelName.Contains(criteria.HotelName));

            if (!string.IsNullOrEmpty(criteria.TemplateId))
                query = query.Where(q => q.TemplateId.Equals(criteria.TemplateId));

            return criteria.NeedPaging ? query.ToPageList(criteria) : query.ToList();
        }

        public int GetSameBrandHotelCount(string brandId)
        {
            return this.Entities.Where(h => h.BrandId == brandId).Count();
        }
    }
}
