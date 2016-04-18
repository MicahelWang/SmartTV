using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;

namespace YeahCentreApi.Controllers
{
    public class UploadFileController : ApiController
    {
        private ILogManager logManager;
        private IQiniuCloudManager qiniuCloudManager;

        public UploadFileController(ILogManager logManager,
         IQiniuCloudManager qiniuCloudManager)
        {
            this.logManager = logManager;
            this.qiniuCloudManager = qiniuCloudManager;
        }

        // GET: UploadFile
        public UpLoadPro GetUploadToken()
        {
            try
            {
                var strToken = qiniuCloudManager.GetUploadToken();
                return strToken;
            }
            catch (Exception ex)
            {
                logManager.SaveError("添加失败", ex, AppType.AppCenter);
                throw new ApiException(ex.Message.ToString());
            }

        }
    }
}