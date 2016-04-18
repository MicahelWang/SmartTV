using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using YeahCatchHotel.Models;

namespace YeahCatchHotel.Util
{
    public abstract class FeachHotelBase
    {
        public string brandType;
        public  string date = DateTime.Now.AddDays(23).ToString("yyyy-MM-dd");

        public string JsonStr { get; set; }
        public FeachHotelBase() { }

        public FeachHotelBase(string brandType)
        {
            this.brandType = brandType;
        }
        /// <summary>
        /// 获取酒店数据模板方法
        /// </summary>
        public void FeachHotelBuzhou()
        {
            if(NeedInitialUrl())
            {
                InitialUrl();
            }
            GetDataSours();
        }
        /// <summary>
        /// 初始化请求URL
        /// </summary>
        /// <param name="hotelType"></param>
        public abstract void InitialUrl();
        /// <summary>
        /// 获取对应品牌未抓取的URL
        /// </summary>
        /// <param name="brandType"></param>
        /// <returns></returns>
        public  List<CatHotelLog> GetHotelList()
        {
            var entitys = new YeahTVEntities();
            var listLog = entitys.CatHotelLogs.Where(f => f.Status == "未抓取" && f.HotelType == brandType).ToList();
            return listLog;
        }
        /// <summary>
        /// 钩子
        /// </summary>
        /// <param name="condition">是否需要初始化地址TrueOrFalse</param>
        /// <returns></returns>
        public virtual bool NeedInitialUrl()
        {
            return true;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <returns></returns>
        public QitianDataModel GetDataSours()
        {
            string url = "";
            HttpClient httpres = new HttpClient();
            var listLog = GetHotelList();
            listLog.AsParallel().ForAll(m => {
                var entity = new YeahTVEntities();
                url = m.Url;
                var obj = entity.CatHotelLogs.FirstOrDefault(h => h.Id == m.Id && h.HotelType == brandType);
                try
                {
                    JsonStr = httpres.Get(url);
                    QitianDataModel o = new QitianDataModel();
                    if (!string.IsNullOrEmpty(JsonStr))
                    {
                        //由子类重写
                        o= HowGetDataSours();
                        if (o.content.Count <= 300)                        //大于300的代表数据有问题
                        {
                            foreach (var item in o.content)
                            {
                                CatchHotel catchHotel = new CatchHotel()
                                {
                                    HoteId = Guid.NewGuid().ToString(),
                                    BrandId = item.brandId,
                                    BrandName = brandType,
                                    districtId = item.districtId,
                                    districtName = item.districtName,
                                    HotelAdress = item.address,
                                    HotelName = item.name,
                                    HotelNameEn = item.firstCharsOfPinyin,
                                    Lat = decimal.Parse(item.blat.ToString()),
                                    Lng = decimal.Parse(item.blng.ToString()),
                                    TokenUrl = url,
                                    InnId = item.innId,
                                    Tel = item.telephone,
                                    LastUpdateTime = DateTime.Now
                                };

                                if (entity.CatchHotels.Any(f => f.InnId == item.innId && f.BrandName == brandType))
                                {
                                    var enHotel = entity.CatchHotels.Where(f => f.InnId == item.innId).FirstOrDefault();
                                    enHotel.HotelAdress = item.address;
                                    enHotel.HotelName = item.name;
                                    enHotel.HotelNameEn = item.firstCharsOfPinyin;
                                    enHotel.Lat = item.blat;
                                    enHotel.Lng = item.blng;
                                    enHotel.Tel = item.telephone;
                                    enHotel.districtId = item.districtId;
                                    enHotel.districtName = item.districtName;
                                    enHotel.BrandId = item.brandId;
                                    enHotel.LastUpdateTime = DateTime.Now;
                                    entity.Entry(enHotel).State = EntityState.Modified;
                                }
                                else
                                {
                                    entity.CatchHotels.Add(catchHotel);
                                }

                            }
                        }
                        obj.Status = "已抓取";
                        entity.Entry(obj).State = EntityState.Modified;
                    }
                    else
                    {
                        obj.Status = "内容为空";
                        entity.Entry(obj).State = EntityState.Modified;
                        entity.SaveChanges();
                    }
                    entity.SaveChanges();
                }
                catch (Exception ex)
                {

                    obj.Token = ex.ToString();
                    obj.Status = "异常";
                    entity.Entry(obj).State = EntityState.Modified;
                    entity.SaveChanges();
                }
            });
            return null;
        }
        /// <summary>
        /// 子类必须重写该方法
        /// </summary>
        /// <returns></returns>
        public abstract QitianDataModel HowGetDataSours();


        public void CheckUrl(string url)
        {
            YeahTVEntities entitys = new YeahTVEntities();
            CatHotelLog catLog = new CatHotelLog()
            {
                HotelType = brandType,
                Status = "未抓取",
                Url = url
            };
            if (!entitys.CatHotelLogs.Any(m => m.Url == url))
            {
                entitys.CatHotelLogs.Add(catLog);
                entitys.SaveChanges();
            }
        }
    }
}