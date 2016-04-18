using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.FactoryInterface;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahTVApiLibrary.Service.HotelMemberInfoManager
{
    public class HotelTypeFactory : IHotelTypeFactory
    {
        private IGlobalConfigManager gloabalConfigManager;
        private IWeiXinService weiXinService;
        private IVODOrderManager vODOrderManager;
        private IOrderQRCodeRecordManager orderQRCodeRecordManager;
        private ILogManager logManager;
        private IRedisCacheManager redisCacheManager;

        public HotelTypeFactory(IGlobalConfigManager gloabalConfigManager, IWeiXinService weiXinService, IVODOrderManager vODOrderManager, IOrderQRCodeRecordManager orderQRCodeRecordManager, ILogManager logManager, IRedisCacheManager redisCacheManager)
        {
            this.gloabalConfigManager = gloabalConfigManager;
            this.weiXinService = weiXinService;
            this.vODOrderManager = vODOrderManager;
            this.orderQRCodeRecordManager = orderQRCodeRecordManager;
            this.redisCacheManager = redisCacheManager;
            this.logManager = logManager;
        }
        public IHotelMemberInfoManager MakeHotelType(string code)
        {
            IHotelMemberInfoManager hotelMemberType = null;
            switch (code)
            {
                case "1000":
                    hotelMemberType = new HotelJjMemberInfoManager(gloabalConfigManager, weiXinService, vODOrderManager, orderQRCodeRecordManager, logManager, redisCacheManager);
                    break;
            }
            return hotelMemberType;
        }

    }
}
