using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using YeahTVApi.Common;
using YeahTVApi.DomainModel;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using YeahTVApi.Entity;
using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Manager;
using System.IO;
using YeahTVApi.DomainModel.Models.DataModel;

namespace YeahTVApiLibrary.UnitTest.YeahTVApiLibrary.ManagerTest
{
    [TestClass]
    public class QiniuCloudManagerTest
    {
        private static readonly string path = System.AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug", "").Replace("bin\\Release", "");

        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<IQiniuCloudService> mockQiniuCloudService;
        private IQiniuCloudManager qiniuCloudManager;

        [TestInitialize]
        public void Setup()
        {
            mockQiniuCloudService = new Mock<IQiniuCloudService>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();

            qiniuCloudManager = new QiniuCloudManager(mockQiniuCloudService.Object, mockConstantSystemConfigManager.Object);
        }

        [TestMethod]
        public void PutFile_ShouldReturnFilePath_WhenGiveValidPutStream()
        {
            //Arrange
            var expectedfileName = Guid.NewGuid().ToString();
            var expectedfileType = "jpg";
            var expectedPath = path + expectedfileName + "." + expectedfileType;
            var memStream = new MemoryStream();

            mockQiniuCloudService.Setup(m => m.PutFile(It.IsAny<Stream>(), It.IsAny<string>())).
                Returns<Stream, string>((stream, fileType) =>
                {
                    return path + expectedfileName + "." + fileType;
                });

            //Act
            var actual = qiniuCloudManager.PutFile(memStream, expectedfileType);

            //Assert
            Assert.AreEqual(expectedPath, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PutFile_ShouldThrowNullReferenceException_WhenPutStreamIsNull()
        {
            //Arrange
            var expectedfileType = "jpg";

            mockQiniuCloudService.Setup(m => m.PutFile(It.Is<Stream>(s => s == null), It.IsAny<string>())).
                Throws(new NullReferenceException());

            //Act
            var actual = qiniuCloudManager.PutFile(null, expectedfileType);
        }

        [TestMethod]
        public void PutFileWithFileName_ShouldReturnFilePath_WhenGiveValidPutStream()
        {
            //Arrange
            var expectedfileName = "TestFileName";
            var expectedfileType = "jpg";
            var expectedPath = path + expectedfileName + "." + expectedfileType;
            var memStream = new MemoryStream();

            mockQiniuCloudService.Setup(m => m.PutFile(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>())).
                Returns<Stream, string, string>((stream, name, fileType) =>
                {
                    return path + name + "." + fileType;
                });

            //Act
            var actual = qiniuCloudManager.PutFile(memStream, expectedfileName, expectedfileType);

            //Assert
            Assert.AreEqual(expectedPath, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void PutFileWithFileName_ShouldThrowNullReferenceException_WhenPutStreamIsNull()
        {
            //Arrange
            var expectedfileName = "TestFileName";
            var expectedfileType = "jpg";

            mockQiniuCloudService.Setup(m => m.PutFile(It.Is<Stream>(s => s == null), It.IsAny<string>(), It.IsAny<string>())).
                Throws(new NullReferenceException());

            //Act
            var actual = qiniuCloudManager.PutFile(null, expectedfileName, expectedfileType);
        }

        [TestMethod]
        public void GetUploadToken_ShouldReturnUpLoadPro_WhenGiveValidQiniuUploadTimeExpires()
        {
            //Arrange 超出max不引发异常
            uint qiniuUploadTimeExpires = uint.MaxValue;
            var expectedUpLoadPro = new UpLoadPro()
            {
                OutTime = DateTime.Now.AddDays(1),
                Token = Guid.NewGuid().ToString()
            };

            mockConstantSystemConfigManager.SetupGet<uint>(s=>s.QiniuUploadTimeExpires).Returns(qiniuUploadTimeExpires);
            mockQiniuCloudService.Setup(m => m.GetUploadToken(It.IsAny<uint>())).Returns(expectedUpLoadPro);

            //Act
            var actual = qiniuCloudManager.GetUploadToken();

            //Assert
            Assert.AreEqual(expectedUpLoadPro, actual);
        }
    }
}
