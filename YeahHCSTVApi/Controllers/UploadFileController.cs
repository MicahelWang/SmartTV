using Qiniu.RS;
using System.Collections.Generic;
using System.Web.Http;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.YeahHcsApi;
using YeahTVApiLibrary.Controllers;
using YeahTVApiLibrary.Filter;
using YeahTVApiLibrary.Infrastructure;

namespace YeahHCSTVApi.Controllers
{
    public class UploadFileController : BaseApiController
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

        [HttpPost]
        [ActionName("GetFileInfo")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = false)]
        public ResponseData<string> GetFileInfo(PostParameters<string> request)
        {
            var fileInfo = qiniuCloudManager.GetFileInfo(request.Data);
            return new ResponseData<string>() { Data = fileInfo };
        }

        [HttpPost]
        [ActionName("GetFilesInfo")]
        [YeahApiCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true, IsCheckDeviceBind = false)]
        public ResponseData<IEnumerable<BatchRetData>> GetFilesInfo(PostParameters<string[]> request)
        {
            var fileInfo = qiniuCloudManager.GetFilesInfo(request.Data);
            return new ResponseData<IEnumerable<BatchRetData>>() { Data = fileInfo };
        }
    }
}