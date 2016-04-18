using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Mapping;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApi.Infrastructure;
using YeahTVApi.Manager;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;
using YeahTVApi.Filter;

namespace YeahTVApi.Controllers
{
    public class HotelTVChannelController:BaseController
    {
        private IHotelTVChannelManager hotelTVChannelManager;
        private ILogManager logManager;

        public HotelTVChannelController(IHotelTVChannelManager hotelTVChannelManager, ILogManager logManager)
        {
            this.hotelTVChannelManager = hotelTVChannelManager;
            this.logManager = logManager;
        }
         [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiListResult<HotelTVChannel> GetTVChannelList(string hotelId)
        {
            var listTVChannel = hotelTVChannelManager.SearchHotelTVChannels(new HotelTVChannelCriteria() { HotelId = hotelId,SortFiled="ChannelOrder" });
            return new ApiListResult<HotelTVChannel> { list = listTVChannel };
        }

         [HTWebFilterAttribute(ShouldNotBindDevice = true)]
         public ApiResult UpdateHotelTVChannelObj(string strhoteltvchannel, string hotelId)
         {
             string error = string.Empty;
             var res = new ApiResult();
             try
             {
                 var lisHoteMovie = strhoteltvchannel.JsonStringToObj<List<HotelTVChannel>>();
                 foreach (var item in lisHoteMovie)
                 {
                     item.HotelId = hotelId;
                     item.LastUpdateUser = Header.Guest;
                     hotelTVChannelManager.UpdateHotelTVChannel(item);
                 }
               /*  list.ForEach(t =>
              {
                  if (lisHoteMovie.Any(e => e.ChannelId.Equals(t.ChannelId) && e.HotelId.Equals(t.HotelId)))
                  {
                      t.LastUpdateTime = DateTime.Now;
                      t.LastUpdateUser = "admin";
                      //t.HostAddress=
                      
                  }
              });*/
                 return res.WithOk();
             }
             catch (Exception ex)
             {
                 logManager.SaveError("保存失败", ex, AppType.CommonFramework);
                 return res.WithError(ex.ToString());
             }
         }
    }
}