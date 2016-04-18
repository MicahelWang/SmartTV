namespace YeahTVApiLibrary.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using YeahTVApi.Common;
    using YeahTVApi.DomainModel;
    using YeahTVApi.DomainModel.Models;
    using YeahTVApi.DomainModel.SearchCriteria;
    using YeahTVApi.Entity;
    using YeahTVApiLibrary.Infrastructure;

    /// <summary>
    /// 
    /// </summary>
    public class HotelTVChannelManager : BaseManager<HotelTVChannel,HotelTVChannelCriteria>, IHotelTVChannelManager
    {
        private IHotelTVChannelRepertory hotelTVChannelRepertory;
        private ITVChannelManager tVChannelManager;
        private ISysAttachmentRepertory sysAttachmentRepertory;
        private ITVHotelConfigRepertory tVHotelConfigRepertory;
        private IConstantSystemConfigManager constantSystemConfigManager;
        private readonly IRedisCacheService _redisCacheService;
        public HotelTVChannelManager(IHotelTVChannelRepertory hotelTVChannelRepertory,
            ITVChannelManager tVChannelManager,
            ISysAttachmentRepertory sysAttachmentRepertory,
            ITVHotelConfigRepertory tVHotelConfigRepertory,
            IConstantSystemConfigManager constantSystemConfigManager,
            IRedisCacheService redisCacheService)
            : base(hotelTVChannelRepertory)
        {
            this.hotelTVChannelRepertory = hotelTVChannelRepertory;
            this.tVChannelManager = tVChannelManager;
            this.sysAttachmentRepertory = sysAttachmentRepertory;
            this.tVHotelConfigRepertory = tVHotelConfigRepertory;
            this.constantSystemConfigManager = constantSystemConfigManager;
            _redisCacheService = redisCacheService;
        }

        [Cache]
        public List<HotelTVChannel> SearchHotelTVChannels(RequestHeader header)
        {
            //var channles = GetAllFromCache().Where(m => m.HotelId.Equals(header.HotelID)).ToList();
            var channles = base.ModelRepertory.Search(new HotelTVChannelCriteria {HotelId = header.HotelID}).ToList();
            channles.ForEach(c =>
            {
                c.HostAddress = @"udp://@" + c.HostAddress + ":8001";

                c.Icon = constantSystemConfigManager.ResourceSiteAddress + sysAttachmentRepertory.FindByKey(int.Parse(c.Icon)).FilePath;
            });

            return channles;
        }


        public List<HotelTVChannel> SearchHotelTVChannels(HotelTVChannelCriteria hotelTVChannelCriteria)
        {
            try
            {
                return base.ModelRepertory.Search(hotelTVChannelCriteria);
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("SearchHotelTVChannels Error!", ex);
            }
        }

        public void AddHotelTVChannel(string hotelId, string lastUpdateUser)
        {
            try
            {
                base.ModelRepertory.Delete(h => h.HotelId.Equals(hotelId));
                var hotelTVChannels = new List<HotelTVChannel>();

                //var tvTemplates = tVChannelManager.GetAllFromCache().OrderBy(t => t.DefaultCode).ToList();
                var tvTemplates = tVChannelManager.Search(new TVChannelCriteria { }).OrderBy(t => t.DefaultCode).ToList();
                var order = 1;

                tvTemplates.ForEach(t =>
                {
                    hotelTVChannels.Add(new HotelTVChannel
                    {
                        Category = t.Category,
                        CategoryEn = t.CategoryEn,
                        ChannelCode = t.DefaultCode,
                        ChannelId = t.Id,
                        ChannelOrder = order,
                        HotelId = hotelId,
                        Icon = t.Icon,
                        LastUpdateTime = DateTime.Now,
                        LastUpdateUser = lastUpdateUser,
                        Name = t.Name,
                        NameEn = t.NameEn,
                        HostAddress = ""
                    });
                    order++;
                });

                base.ModelRepertory.Insert(hotelTVChannels);

            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("AddHotelTVChannel Error!", ex);
            }
        }



        public void AddHotelTVChannel(HotelTVChannel hotelTVChannel)
        {
            try
            {
                base.ModelRepertory.Insert(hotelTVChannel);
                
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("AddHotelTVChannel Error!", ex);
            }
        }
        public void UpdateHotelTVChannel(HotelTVChannel hotelTVChannel)
        {
            try
            {
                var hotelTVChannels = base.ModelRepertory.Search(new HotelTVChannelCriteria { HotelId = hotelTVChannel.HotelId });
                var hotelTVChannelDb = hotelTVChannels.FirstOrDefault(m => m.ChannelId == hotelTVChannel.ChannelId);
                hotelTVChannelDb.ChannelOrder = hotelTVChannel.ChannelOrder;
                hotelTVChannelDb.HostAddress = hotelTVChannel.HostAddress;
                hotelTVChannelDb.LastUpdateTime = DateTime.Now;
                hotelTVChannelDb.LastUpdateUser = hotelTVChannel.LastUpdateUser;

                base.ModelRepertory.Update(hotelTVChannelDb);

            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("UpdateHotelTVChannel Error!", ex);
            }
        }

        public void DeleteHotelTVChannel(HotelTVChannel hotelTVChannel)
        {
            try
            {
                base.ModelRepertory.Delete(h => h.HotelId.Equals(hotelTVChannel.HotelId) && h.ChannelId.Equals(hotelTVChannel.ChannelId));
            }
            catch (Exception ex)
            {
                throw new CommonFrameworkManagerException("UpdateHotelTVChannel Error!", ex);
            }
        }

        public List<string> SearchOnlyHotelId(BaseSearchCriteria searchCriteria)
        {
            return hotelTVChannelRepertory.SearchOnlyHotelId(searchCriteria);
        }
    }
}
