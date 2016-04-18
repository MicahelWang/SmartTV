using Qiniu.IO;
using Qiniu.IO.Resumable;
using Qiniu.RS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.DomainModel;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApi.Common;

namespace YeahTVApiLibrary.Service
{
    public class QiniuCloudService : IQiniuCloudService
    {
        private static readonly string path = System.AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "").Replace("bin\\Release", "");

        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Bucket { get; set; }

        public string PutFile(Stream putStream, string fileType)
        {
            return PutFile(putStream, Guid.NewGuid().ToString(), fileType);
        }

        public string PutFile(Stream putStream, string fileName, string fileType)
        {
            var filePath = path + fileName + "." + fileType;
            SaveFileToLocal(filePath, putStream);
            Qiniu.Conf.Config.ACCESS_KEY = AccessKey;
            Qiniu.Conf.Config.SECRET_KEY = SecretKey;

            var policy = new PutPolicy(Bucket, 3600);
            var upToken = policy.Token();
            var extra = new PutExtra();
            var client = new IOClient();
            var respone = client.PutFile(upToken, fileName + "." + fileType, filePath, extra);
            //上传完文件本地副本必须删除
            //DeleteLocalFile(filePath);
            //end
            return respone.key;
        }


        public UpLoadPro GetUploadToken(uint timeExpires)
        {
            UpLoadPro pro = new UpLoadPro();
            Qiniu.Conf.Config.ACCESS_KEY = AccessKey;
            Qiniu.Conf.Config.SECRET_KEY = SecretKey;
            var policy = new PutPolicy(Bucket, timeExpires * 60000);
            pro.Token = policy.Token();
            pro.OutTime = DateTime.Parse(DateTime.Now.AddMinutes(timeExpires).ToString("yyyy-MM-dd HH:mm:ss"));
            return pro;
        }

        public void DeleteLocalFile(string filePath)
        {
            File.Delete(filePath);
        }

        public string GetFileInfo(string fileName)
        {
            Qiniu.Conf.Config.ACCESS_KEY = AccessKey;
            Qiniu.Conf.Config.SECRET_KEY = SecretKey;
            var client = new RSClient();
            var entry = client.Stat(new EntryPath(Bucket, fileName));
            if (entry.OK)
            {
                return entry.ToJsonString();
            }
            else
            {
                return "Error!";
            }
        }

        public IEnumerable<BatchRetData> GetFilesInfo(string[] keys)
        {
            Qiniu.Conf.Config.ACCESS_KEY = AccessKey;
            Qiniu.Conf.Config.SECRET_KEY = SecretKey;
            var client = new RSClient();
            var EntryPaths = new List<EntryPath>();
            foreach (var key in keys)
            {
                EntryPaths.Add(new EntryPath(Bucket, key));
            }
            var filesInfo = client.BatchStat(EntryPaths.ToArray()).Select(c=>c.data);
            return filesInfo;
        }

        private void SaveFileToLocal(string filePath, Stream putStream)
        {
            var sourceStream = putStream;
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var bufferLength = sourceStream.Length;
                byte[] myBuffer = new byte[bufferLength];//数据缓冲区
                int count;
                while ((count = sourceStream.Read(myBuffer, 0, (int)bufferLength)) > 0)
                {
                    fs.Write(myBuffer, 0, count);
                }
                fs.Close();
                sourceStream.Close();
            }
        }
      
    }
}
