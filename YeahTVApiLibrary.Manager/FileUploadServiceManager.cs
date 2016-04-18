using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeahTVApiLibrary.Infrastructure;
using System.IO;

namespace YeahTVApiLibrary.Manager
{
    public class FileUploadServiceManager : IFileUploadServiceManager
    {
        private IImageUpdateServiceProvider imageUpdateService;

        public FileUploadServiceManager(IImageUpdateServiceProvider imageUpdateService)
        {
            this.imageUpdateService = imageUpdateService;
        }


        public string UpdateImageByBitmapStream(Stream imageBitmapStream,string fileType)
        {
            return imageUpdateService.UpdateImageByBitmapStream(imageBitmapStream, fileType);
        }

        public string UpdateImageByBase64String(string base64String, string fileType)
        {
            byte[] arr = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(arr);

            return imageUpdateService.UpdateImageByBitmapStream(ms, fileType);
        }

        public string UpdateAppByStream(Stream imageBitmapStream, string fileType)
        {
            return imageUpdateService.UpdateAppByStream(imageBitmapStream, fileType);
        }
    }
}
