using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models.DomainModels;
using YeahTVApi.DomainModel.Models.ScoreStoreModels;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;

namespace YeahTVApiLibrary.Manager
{
    public class OpenApiManager : IOpenApiManager
    {
        private IConstantSystemConfigManager constantSystemConfigManager;

        public OpenApiManager(IConstantSystemConfigManager constantSystemConfigManager)
        {
            this.constantSystemConfigManager = constantSystemConfigManager;
        }

        //验证token
        public bool VerificationToken(RequestTokenParameter requestTokenParameter)
        {
            BaseReturnMessage result = new BaseReturnMessage();
            string data = string.Format("ticket={0}&code={1}&token={2}", requestTokenParameter.Ticket,
                    requestTokenParameter.Code, requestTokenParameter.Token);

            var url = constantSystemConfigManager.OpenApiAddress + "/" + Constant.VerificationTokenUrl;
            // url = "http://192.168.8.11:8066/" + Constant.VerificationTokenUrl;//

            var responsedata = (new HttpHelper { }).Post(url, data, "Post");


            if (!string.IsNullOrWhiteSpace(responsedata))
                result = JsonConvert.DeserializeObject<BaseReturnMessage>(responsedata);

            if (result.Code ==(int)ApiErrorType.Success)
                return true;
            else
                return false;
        }
    }
}
