using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;
using Moq;
using System.Web;

namespace YeahResourceApi.Service.Tests
{
    [TestClass]
    public class ImageUpdateTest
    {
        private IImageUpdate imageUpdate;

        [TestInitialize]
        public void SetUp()
        {
            imageUpdate = new ImageUpdate();
        }

        [TestMethod]
        public void UpdateImageByBitmapStream_ShouldReturnFileName_WhenSaveImageSuccessfull()
        {
            // Arrange

            var path = System.AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "").Replace("bin\\Release", "") + "YeahTVDB.png";
            var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]  
            var bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            
            // 把 byte[] 转换成 Stream  
            var stream = new MemoryStream(bytes);

            // Act 
            var actual = imageUpdate.UpdateImageByBitmapStream(new RemoteFileInfo { FileByteStream = stream, FileType = "jpg", FileLength = stream.Length });

            fileStream.Close();
            // Assert
        }
    }
}
