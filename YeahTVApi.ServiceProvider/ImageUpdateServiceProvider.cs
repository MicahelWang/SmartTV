using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApi.ServiceProvider.UpdateImages;
using YeahTVApiLibrary.Infrastructure;

namespace YeahTVApi.ServiceProvider
{
    public class ImageUpdateServiceProvider : IImageUpdateServiceProvider
    {
        IImageUpdate imageUpdate = new ImageUpdateClient();

        public string UpdateImageByBitmapStream(Stream imageBitmapStream,string fileType)
        {
            return imageUpdate.UpdateImageByBitmapStream(new RemoteFileInfo { FileByteStream = imageBitmapStream, FileType = fileType, FileLength = imageBitmapStream.Length}).FileName;
        }

        public string UpdateAppByStream(Stream imageBitmapStream, string fileType)
        {
            return imageUpdate.UpdateAppByStream(new RemoteFileInfo { FileByteStream = imageBitmapStream, FileType = fileType, FileLength = imageBitmapStream.Length }).FileName;
        }
    }
}
