using Qiniu.RS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.DomainModel.Models.ViewModels;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IWeiXinService
    {
        Tuple<PaymentResponseInfo,string> CreateQrcode(string accessToken, string sceneId);
    }
}
