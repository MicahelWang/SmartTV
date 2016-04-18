using System;
using YeahTVApi.DomainModel.Enum;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.Entity;
using YeahTVApi.Filter;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApi.Controllers
{
    public class UploadFileController : BaseController
    {
        // GET: UnloadFile
        private ILogManager logManager;
        private IQiniuCloudManager qiniuCloudManager;

        public UploadFileController(ILogManager logManager,
         IQiniuCloudManager qiniuCloudManager)
        {
            this.logManager = logManager;
            this.qiniuCloudManager = qiniuCloudManager;
        }

        [HTWebFilterAttribute(ShouldNotBindDevice = true)]
        public ApiObjectResult<UpLoadPro> GetUploadToken()
        {
            try
            {
                var strToken = qiniuCloudManager.GetUploadToken();
                return new ApiObjectResult<UpLoadPro>() { obj = strToken };
            }
            catch (Exception ex)
            {
                logManager.SaveError("添加失败", ex, AppType.CommonFramework);
                throw new ApiException(ex.Message.ToString());
            }

        }

        public ApiObjectResult<object> GetFileInfo(string fileName)
        {
            var fileInfo = qiniuCloudManager.GetFileInfo(fileName);
            return new ApiObjectResult<object>() { obj = fileInfo };
        }
    }
}