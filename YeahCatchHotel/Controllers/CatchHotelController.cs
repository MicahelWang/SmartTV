using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YeahCatchHotel.Models;
using YeahCatchHotel.Util;
using YeahTVApi.Common;

namespace YeahCatchHotel.Controllers
{
    public class CatchHotelController : Controller
    {
        //
        // GET: /CatchHotel/
       

        public ActionResult Index()
        {
            return View("");
        }

        public ActionResult CatchSevenDay(string brandType)
        {
            var entitys = new YeahTVEntities();
            HttpClient httpres = new HttpClient();
            string url = "";
            var listLog = entitys.CatHotelLogs.Where(f => f.Status == "未抓取" && f.HotelType == brandType).ToList();
            //listLog.AsParallel().ForAll(m =>
            listLog.ForEach(m=>
            {
                var entity = new YeahTVEntities();
                url = m.Url;
                var obj = entity.CatHotelLogs.FirstOrDefault(h => h.Id == m.Id&&h.HotelType==brandType);
                try
                {
                    string jsonStr = httpres.Get(url);
                    QitianDataModel o = new QitianDataModel();
                    if (!string.IsNullOrEmpty(jsonStr))
                    {
                        switch (brandType)
                        {
                            case "七天":
                                o = JsonConvert.DeserializeObject<QitianDataModel>(jsonStr);
                                break;
                            case "速八":

                                string[] arr = jsonStr.Split('@');
                                List<QitianHotelDataModel> list = new List<QitianHotelDataModel>();
                                for (int i = 0; i < arr.Length; i++)
                                {
                                    string[] arrObj = arr[i].Split('|');
                                    QitianHotelDataModel objc = new QitianHotelDataModel()
                                    {
                                        name = arrObj[0].Trim(),
                                        address = arrObj[1].Trim(),
                                        telephone = arrObj[2].Trim(),
                                        blng = decimal.Parse(arrObj[4].Trim()),
                                        blat = decimal.Parse(arrObj[5].Trim()),
                                        innId = arrObj[6].Trim()
                                    };
                                    list.Add(objc);
                                }
                                o.content = list;
                                break;
                            default:
                                break;
                        }


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
                                    var enHotel = entitys.CatchHotels.Where(f => f.InnId == item.innId).FirstOrDefault();
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
                    entitys.Entry(obj).State = EntityState.Modified;
                    entitys.SaveChanges();
                }
            });
            return Content("<script>alert('获取成功')</script>");
        }


        public ActionResult CatchSuba()
        {
            InitialSubaUrl();
            return Content("<script>alert('获取成功')</script>");
        }

        public void InitialSubaUrl()
        {
            Initial("速八");
            /* var arr=System.IO.File.ReadAllText(@"C:\Users\tianyu\Desktop\SubaCity.txt",System.Text.Encoding.UTF8).Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);*/
        }

        public void Initial(string hotelType)
        {
            string[] citys = new string[] { "北京", "通辽市", "阿拉善盟", "莱阳", "咸宁市", "中山", "合作市", "广州", "上海", "西安", "杭州", "青岛", "沈阳", "长春", "南京", "北京", "阿拉善盟", "长春", "成都", "大连", "安溪", "德阳", "赤壁", "巴彦淖尔", "潮州", "亳州", "赤峰", "白银", "滁州市", "长乐", "敦煌市", "宝鸡", "安康", "北海市", "朝阳", "东阳市", "东营", "安阳", "巴音郭楞", "蚌埠", "滨州", "包头", "常熟", "定西", "承德", "大同", "池州", "保定", "安丘", "德州", "长沙", "东莞", "赤水", "重庆", "常州", "白山", "合作市", "广州", "杭州", "福州", "广安", "济南", "哈尔滨", "奉化市", "鄂尔多斯", "吉林", "呼和浩特", "嘉峪关市", "福鼎市", "阜阳", "桂林", "哈密市", "固原", "晋江市", "金坛市", "衡水市", "呼伦贝尔", "海阳市", "海安", "介休市", "惠州", "峨眉山市", "将乐县", "邯郸", "锦州", "淮北", "福安市", "葫芦岛", "酒泉", "合肥", "靖江", "黄石", "焦作", "黄山", "贵阳", "金华", "华阴", "淮安", "菏泽", "济宁", "佛山", "福清", "抚松", "番禺", "莱阳", "南京", "兰州", "绵阳", "阆中", "宁波", "聊城", "喀什", "南平市", "临夏市", "泸州", "乐山", "陇南", "廊坊", "吕梁", "昆山市", "龙岩", "南昌", "临沂", "宁德", "连云港", "克拉玛依", "开封", "南通", "拉萨", "洛阳", "南宁", "龙口", "通辽市", "上海", "青岛", "沈阳", "天津", "泰安", "苏州", "石狮市", "唐山", "太仓", "商丘市", "迁安", "泰州", "台山", "汕头", "平凉市", "石嘴山", "石河子", "乳山", "庆阳", "上饶", "天长市", "石家庄", "十堰", "濮阳", "秦皇岛", "日照", "宿迁", "太原", "遂宁", "盘锦", "三亚", "泉州", "莆田", "千岛湖", "天水", "深圳", "三明", "咸宁市", "中山", "西安", "厦门", "岳阳", "武汉", "镇江", "西宁", "西昌", "邹城市", "枣庄", "武夷山市", "兴城", "义乌", "榆林", "西双版纳", "盐城", "邢台", "温州", "扬州", "徐州", "淄博", "无锡", "芜湖", "武威", "威海", "诸城", "仪征", "潍坊", "自贡", "银川", "永安", "烟台", "中卫", "乌鲁木齐", "珠海", "延安", "张家口", "郑州", "许昌", "伊宁", "伊宁" };
            string url = "";
            string date = DateTime.Now.AddDays(23).ToString("yyyy-MM-dd");
            switch (hotelType)
            {
                case "七天":
                    for (int i = 1; i <= 604; i++)
                    {
                        url = "http://www.plateno.com/hotel/q/list?cityId=" + i.ToString() + "&fromDate=" + date + "&days=1&sortFlag=0&fetchRt=true&intlSearch=false&fromType=0&pageNumber=1&pageSize=300&checkCount=false&_=1433502762073";
                        CheckUrl(hotelType, url);
                    }
                    break;
                case "速八":
                    for (int i = 0; i < citys.Length; i++)
                    {
                        url = "http://www.super8.com.cn/Ajax/HotelInterface.ashx?action=mapabc&sdate=" + date + "&edate=" + DateTime.Parse(date).AddDays(1).ToString("yyyy-MM-dd") + "&cityName=" + Url.Encode(citys[i]) + "&keywords=%E9%85%92%E5%BA%97%E5%90%8D%E7%A7%B0%2F%E5%9C%B0%E5%9D%80%2F%E8%A1%8C%E6%94%BF%E5%8C%BA%E5%9F%9F%E7%AD%89";
                        CheckUrl(hotelType, url);
                    }
                    break;
                default:
                    break;
            }

        }


        public void CheckUrl(string hotelType, string url)
        {
            YeahTVEntities entitys = new YeahTVEntities();
            CatHotelLog catLog = new CatHotelLog()
            {
                HotelType = hotelType,
                Status = "未抓取",
                Url = url
            };
            if (!entitys.CatHotelLogs.Any(m => m.Url == url))
            {
                entitys.CatHotelLogs.Add(catLog);
                entitys.SaveChanges();
            }
        }


        public void Inser()
        {
            HttpClient httpres = new HttpClient();
           string json=  httpres.Post("http://www.wyn88.com/hotel/searchHotelListJson.html",
                string.Format("cityName={0}&startTime={1}&endTime={2}&keyWord=&keyType=", "深圳市", "2015-06-30", "2015-07-01"));
        }
    }
}
