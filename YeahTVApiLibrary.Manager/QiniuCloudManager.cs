using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models.DataModel;

namespace YeahTVApiLibrary.Manager
{
    public class QiniuCloudManager : IQiniuCloudManager
    {
        private IQiniuCloudService qiniuCloudService;
        private IConstantSystemConfigManager constantSystemConfigManager;

        public QiniuCloudManager(IQiniuCloudService qiniuCloudService,IConstantSystemConfigManager constantSystemConfigManager)
        {
            this.qiniuCloudService = qiniuCloudService;
            this.constantSystemConfigManager = constantSystemConfigManager;
            qiniuCloudService.Bucket = constantSystemConfigManager.QinuiBucket;
            qiniuCloudService.AccessKey = constantSystemConfigManager.QinuiAk;
            qiniuCloudService.SecretKey = constantSystemConfigManager.QinuiSk;
        }

        public string PutFile(Stream putStream, string fileType)
        {
            return qiniuCloudService.PutFile(putStream, fileType);
        }

        public string PutFile(Stream putStream, string fileName, string fileType)
        {
            return qiniuCloudService.PutFile(putStream, fileName, fileType);
        }


        public UpLoadPro  GetUploadToken()
        {
            return qiniuCloudService.GetUploadToken(constantSystemConfigManager.QiniuUploadTimeExpires);
        }

        public string GetFileInfo(string fileName)
        {
            return qiniuCloudService.GetFileInfo(fileName);
        }

        public IEnumerable<Qiniu.RS.BatchRetData> GetFilesInfo(string[] keys)
        {
            return qiniuCloudService.GetFilesInfo(keys);
        }
    }
}
