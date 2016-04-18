using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.Common;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Enum;

namespace YeahCentre.Manager
{
    public class HotelPermitionManager : IHotelPermitionManager
    {
        private IHotelPermitionRepertory hotelPermitionRepertory;
        private IRedisCacheService redisCacheService;
        private readonly IHotelManager _hotelManager;
        private readonly IBrandManager _brandManager;

        public HotelPermitionManager(IHotelPermitionRepertory hotelPermitionRepertory,
            IHotelManager hotelManager
            , IRedisCacheService redisCacheService
            , IBrandManager brandManager)
        {
            this.hotelPermitionRepertory = hotelPermitionRepertory;
            this.redisCacheService = redisCacheService;
            _hotelManager = hotelManager;
            _brandManager = brandManager;
        }

        #region Redis

        public List<HotelPermition> GetAllFromCache()
        {
            return redisCacheService.GetAllFromCache(RedisKey.HotelPermitionKey, hotelPermitionRepertory.GetAll);
        }

        public void RemoveCache()
        {
            redisCacheService.Remove(RedisKey.HotelPermitionKey);
        }

        #endregion

        [UnitOfWork]
        public void InsertAll(List<HotelPermition> listOjb)
        {
            hotelPermitionRepertory.Insert(listOjb);
            RemoveCache();
        }

        [UnitOfWork]
        public void DelelteByUserId(string userId)
        {
            hotelPermitionRepertory.Delete(m => m.UserId.Equals(userId));
            RemoveCache();
        }

        public List<HotelPermition> GetHotelByUserId(string userId)
        {
            return GetAllFromCache().Where(u => u.UserId == userId).ToList();
        }

        public List<HotelPermition> Search(HotelPermitionCriteria searchCriteria)
        {
            return hotelPermitionRepertory.Search(searchCriteria);
        }

        public List<CoreSysHotel> GetHotelListByPermition(string uid)
        {
            var permitionList = GetHotelByUserId(uid);
            var allHotel = _hotelManager.GetAllCoreSysHotels();
            var GroupIdList =
                permitionList.Where(o => o.PermitionType.Equals(PermitionEnum.Group.ToString()))
                    .Select(o => o.TypeId)
                    .ToList();
            var BrandIdList =
                permitionList.Where(o => o.PermitionType.Equals(PermitionEnum.Brand.ToString()))
                    .Select(o => o.TypeId)
                    .ToList();
            var HotelIdList =
                permitionList.Where(o => o.PermitionType.Equals(PermitionEnum.Hotel.ToString()))
                    .Select(o => o.TypeId)
                    .ToList();
            var listResult = new List<CoreSysHotel>();
            if (GroupIdList.Count > 0)
            {
                var query = allHotel.Where(m => GroupIdList.Contains(m.GroupId)).ToList();
                listResult.AddRange(query);
            }
            if (BrandIdList.Count > 0)
            {
                var query = allHotel.Where(m => BrandIdList.Contains(m.BrandId)).ToList();
                listResult.AddRange(query);
            }
            if (HotelIdList.Count > 0)
            {
                var query = allHotel.Where(m => HotelIdList.Contains(m.Id)).ToList();
                listResult.AddRange(query);
            }
            return listResult;
        }

        public Dictionary<string, string> GetBrandConfigUrlsByUserId(string userId)
        {
            var urls = new Dictionary<string, string>();
            var brandList = new List<CoreSysBrand>();

            var permitionList = GetHotelByUserId(userId);
            var allBrand = _brandManager.GetAll();
            var groupIdList =
                permitionList.Where(o => o.PermitionType.Equals(PermitionEnum.Group.ToString()))
                    .Select(o => o.TypeId)
                    .ToList();
            var brandIdList =
                permitionList.Where(o => o.PermitionType.Equals(PermitionEnum.Brand.ToString()))
                    .Select(o => o.TypeId)
                    .ToList();

            if (groupIdList.Count > 0)
            {
                var query = allBrand.Where(m => groupIdList.Contains(m.GroupId)).ToList();
                brandList.AddRange(query);
            }
            if (brandIdList.Count > 0)
            {
                var query = allBrand.Where(m => brandIdList.Contains(m.Id)).ToList();
                brandList.AddRange(query);
            }

            var brandUrls = ReadBrandUrlFromXml();
            string defaultUrl;
            GetDefaultUrl().TryGetValue("1.0", out defaultUrl);

            brandList.ForEach(m =>
            {
                string tempUrl;
                if (!brandUrls.TryGetValue(m.Id, out tempUrl))
                    tempUrl = defaultUrl;

                if (!string.IsNullOrWhiteSpace(tempUrl))
                    urls.Add(m.BrandName, tempUrl + "?brandId=" + m.Id);
            });


            return urls;
        }

        private Dictionary<string, string> ReadBrandUrlFromXml()
        {
            var hotelCustomizing = new Dictionary<string, string>();
            var defaultUrl = GetDefaultUrl();
            var element =
                XElement.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/SiteXml/Constant.xml"));
            element.Element("HotelCustomizing")
                .Elements("Node")
                .Where(m => !string.IsNullOrWhiteSpace(m.Attribute("flag").Value)).ToList().ForEach(m =>
                {
                    if (string.IsNullOrWhiteSpace(m.Value))
                    {
                        string tempUrl;
                        if (defaultUrl.TryGetValue(m.Attribute("versionCode").Value, out tempUrl))
                        {
                            hotelCustomizing.Add(m.Attribute("flag").Value, tempUrl);
                        }
                        else
                            throw new Exception(string.Format("未配置版本{0}默认URL！", m.Attribute("versionCode").Value));
                    }
                    else
                    {
                        hotelCustomizing.Add(m.Attribute("flag").Value, m.Value);
                    }
                });

            return hotelCustomizing;
        }

        private Dictionary<string, string> GetDefaultUrl()
        {
            var defaultUrl = new Dictionary<string, string>();
            //defaultUrl.Add("1.0", "/DashBoard/IUConfigIndex");
            return defaultUrl;
        }
    }
}
