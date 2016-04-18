using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models;

namespace YeahTVApiLibrary.Infrastructure.RepositoriesInterface.IRepertory
{
    public interface IStoreOrderRepertory:IBsaeRepertory<StoreOrder>
    {
        string GetNewOrderId(string hotelCode, string orderType);
    }
}
