namespace YeahTVApi.ServiceProvider
{
    using YeahTVApi.Entity;
    using YeahTVApi.Infrastructure;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CheckInService : ICheckInService
    {
        private AutoCheckInService.WebSelfHelpCheckInClient client = null;

        public AutoCheckInService.WebSelfHelpCheckInClient ServiceClient
        {
            get
            {
                if (client == null)
                {
                    client = new AutoCheckInService.WebSelfHelpCheckInClient();
                    client.Open();
                }

                return client;
            }
        }
        
        /// <summary>
        /// 获取制卡数据
        /// </summary>
        /// <param name="hotelId">酒店ID</param>
        /// <param name="receiveId">接待单号</param>
        /// <returns></returns>
        public List<CardData> GetCardData(string hotelId, string roomNumber, string CardSNO, List<Guest> guests)
        {
            var list = new List<CardData>();

            string receiveID = null;
            foreach (var guest in guests)
            {
                if (guest.ReceiveID != null)
                {
                    receiveID = guest.ReceiveID;
                    break;
                }
            }

            if (string.IsNullOrEmpty(receiveID))
                return list;
            
            string systemPw = "", wirteKey = "", building = "", floor = "", room = "", batchNo = "", waterno = "", ExpectedDate = "";
            var info = ServiceClient.GetWriteCardInfo(hotelId, roomNumber, receiveID);
            foreach (var entity in info)
            {
                if (entity.Key == "systemPw")
                    systemPw = entity.Value;
                else if (entity.Key == "wirteKey")
                    wirteKey = entity.Value;
                else if (entity.Key == "buildingName")
                    building = entity.Value;
                else if (entity.Key == "floorName")
                    floor = entity.Value;
                else if (entity.Key == "room")
                    room = entity.Value;
                else if (entity.Key == "p")
                    batchNo = entity.Value;
                else if (entity.Key == "waterno")
                    waterno = entity.Value;
                else if (entity.Key == "ExpectedDate")
                    ExpectedDate = entity.Value;
            }

            //获取会员的最晚离店时间
            DateTime EndDate = DateTime.Parse(ExpectedDate);
            if (EndDate.Minute == 0)
            {
                EndDate = EndDate.AddMinutes(30);
            }
            else
            {
                EndDate = EndDate.AddMinutes(31);
            }
            EndDate = EndDate.AddDays(10);
            room = room.Substring(room.Length - 2);
            byte[] data = new CardGen().GetKexinData(systemPw, //系统密码
              int.Parse(building.Split('|')[1]),  //楼
              int.Parse(floor.Split('|')[1]),  //层
              int.Parse(room),  //房间
              int.Parse(batchNo),   //批次号
              DateTime.Now,
               EndDate,
              int.Parse(waterno));

            var item = new CardData();

            byte[] d1 = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                d1[i] = data[i];
            }
            item.byteData = d1;
            item.RoomNO = roomNumber;
            item.HotelID = hotelId;
            //item.Data = Convert.ToBase64String(d1);
            foreach (var b in d1)
            {
                item.Data += b.ToString("X2");
            }

            item.Block = 1;
            item.Sector = 0;
            item.WritePassword = wirteKey;
            item.EndDate = EndDate.ToString("yyyy-MM-dd");
            list.Add(item);

            item = new CardData();
            d1 = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                d1[i] = data[i + 16];
            }
            item.byteData = d1;
            //item.Data = Convert.ToBase64String(d1);
            foreach (var b in d1)
            {
                item.Data += b.ToString("X2");
            }
            item.Block = 2;
            item.Sector = 0;
            item.WritePassword = wirteKey;
            item.EndDate = EndDate.ToString("yyyy-MM-dd");
            list.Add(item);

            if (receiveID != null)
            {

                string shift = "2", OperatorID = "智能门锁";
                var lockCard = new LockCardEntity();
                lockCard.CardNo = batchNo;
                //卡序列号
                lockCard.CardSnr = CardSNO;
                //卡类别
                /*
                01 紧急卡
                02 总控卡
                03 员工卡
                04 服务卡
                05A    服务卡A
                05B    服务卡B
                05C    服务卡C
                06 员工卡OLD
                07 常开卡
                08 一次卡
                09 备份卡
                10 密码卡
                11 房号卡
                12 时钟卡
                13 设置卡
                14 总清卡
                15 禁止卡
                16 挂失卡
                17 解挂卡
                18 记录卡
                20 客人卡
                21 预定卡
                30 扇区卡
                31 原始卡
                 * */
                lockCard.CardTypeID = "20"; //这里一般为20
                lockCard.HotelID = hotelId;
                lockCard.ReceiveID = receiveID;
                lockCard.RoomNo = roomNumber;
                lockCard.Shift = shift;
                lockCard.sOperatorID = OperatorID;
                lockCard.SpecialRoom = "";
                lockCard.sOperatorName = OperatorID;
                lockCard.DepDate = EndDate;
                lockCard.DepDateSpecified = true;
                String WriteContend = "";

                var writeData = list[0].byteData.Concat(list[1].byteData);
                foreach (var b in writeData)
                {
                    WriteContend += b.ToString("X2");
                }
                lockCard.WriteContend = WriteContend;
            }

            return list;
        }
    }
}
