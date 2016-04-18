using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahTVApiLibrary.Infrastructure.FactoryInterface
{
   public  interface IHotelTypeFactory
    {
       IHotelMemberInfoManager MakeHotelType(string code);
    }
}
