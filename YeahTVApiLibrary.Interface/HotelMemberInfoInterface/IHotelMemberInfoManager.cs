using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahTVApiLibrary.Infrastructure.ManagerInterface
{
   public interface IHotelMemberInfoManager
    {
        ScorePointResult GetTheUserScore(string hotelId, params string[] parameters);
        PaymentResponseInfo GetScoreQRCode(string orderId, string hotelId);

        Tuple<ScorePointResult, RequestJjData> CommitOrderRequest(RequestScore requestScore, YeahTVApi.DomainModel.Models.VODOrder vodOrder, decimal userScore);
    }
}
