using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Web;

namespace YeahResourceApi.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ImageUpdate" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ImageUpdate.svc or ImageUpdate.svc.cs at the Solution Explorer and start debugging.
    public class ImageUpdate : IImageUpdate
    {
        private readonly string path = System.AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "").Replace("bin\\Release", "");
        
        public UpLoadRequest UpdateImageByBitmapStream(RemoteFileInfo remoteFileInfo)
        {
            try
            {
                return UpLoadFile(remoteFileInfo, "ImagePath");
            }
            catch
            {
                return new UpLoadRequest();
            }
        }

        public UpLoadRequest UpdateAppByStream(RemoteFileInfo remoteFileInfo)
        {
            try
            {

                return UpLoadFile(remoteFileInfo, "AppPath");
            }
            catch
            {
                return new UpLoadRequest();
            }
        }

        private  UpLoadRequest UpLoadFile(RemoteFileInfo remoteFileInfo, string appSettingName)
        {
            var fileName = string.Empty;
            fileName = ConfigurationManager.AppSettings[appSettingName] + Guid.NewGuid().ToString() + "." + remoteFileInfo.FileType;

            Stream sourceStream = remoteFileInfo.FileByteStream;

            //创建文件流，读取流中的数据生成文件
            using (FileStream fs = new FileStream(path + fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var bufferLength = remoteFileInfo.FileLength;
                byte[] myBuffer = new byte[bufferLength];//数据缓冲区
                int count;
                while ((count = sourceStream.Read(myBuffer, 0, (int)bufferLength)) > 0)
                {
                    fs.Write(myBuffer, 0, count);
                }
                fs.Close();
                sourceStream.Close();
            }

            return new UpLoadRequest { FileName = fileName.Replace("\\", "/"), IsUpLoad = true };
        }
    }
}
