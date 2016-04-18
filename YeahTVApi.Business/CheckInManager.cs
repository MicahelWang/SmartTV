namespace HZTVApi.Manager
{
    using HZTVApi.Entity;
    using HZTVApi.Infrastructure;
    using HZTVApi.Resource;
    using System;
    using System.Linq;

    public class CheckInManager : ICheckInManager
    {
        private ICheckInService checkInService;
        private IGetGuestInfoService guestInfoService;

        public CheckInManager(ICheckInService checkInService, IGetGuestInfoService guestInfoService)
        {
            this.checkInService = checkInService;
            this.guestInfoService = guestInfoService;
        }

        /// <summary>
        /// 获取制卡数据
        /// </summary>
        /// <param name="hotelId">酒店ID</param>
        /// <param name="receiveId">接待单号</param>
        /// <returns></returns>
        public ApiListResult<CardData> GetCardData(string hotelId, String roomNumber, String cardSNO)
        {
            var result = new ApiListResult<CardData>();
            if (hotelId == null || roomNumber == null)
            {
                result.WithError(HZTVApiMessage.CommonNoRoomNo);
                return result;
            }
            var guests = guestInfoService.GetInfoList(hotelId, roomNumber);
            if (guests == null || !guests.Any())
            {
                result.WithError(roomNumber + HZTVApiMessage.CommonNoCheckIn);
                return result;
            }

            var cardDatas = checkInService.GetCardData(hotelId, roomNumber, cardSNO, guests);

            if (cardDatas == null || !cardDatas.Any())
            {
                result.WithError(roomNumber + HZTVApiMessage.CommonNoCheckIn);
                return result;
            }

            result.WithOk(cardDatas);

            return result;
        }
    }
}
