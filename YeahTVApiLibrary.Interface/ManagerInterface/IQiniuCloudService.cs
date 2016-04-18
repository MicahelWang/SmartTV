using Qiniu.RS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel.Models.DataModel;

namespace YeahTVApiLibrary.Infrastructure
{
    public interface IQiniuCloudService
    {
        string AccessKey { get; set; }
        string SecretKey { get; set; }
        string Bucket { get; set; }

        string PutFile(Stream putStream, string fileType);

        string PutFile(Stream putStream, string fileName, string fileType);

        UpLoadPro GetUploadToken(uint timeExpires);

        string GetFileInfo(string fileName);
        IEnumerable<BatchRetData> GetFilesInfo(string[] keys);
    }
}
