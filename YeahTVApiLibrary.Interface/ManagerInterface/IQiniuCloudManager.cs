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
    public interface IQiniuCloudManager
    {
        string PutFile(Stream putStream, string fileType);

        string PutFile(Stream putStream, string fileName, string fileType);

        UpLoadPro GetUploadToken();
        string GetFileInfo(string fileName);
        IEnumerable<BatchRetData> GetFilesInfo(string[] keys);
    }
}
