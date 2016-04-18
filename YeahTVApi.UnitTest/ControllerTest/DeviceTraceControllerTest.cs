using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApiLibrary.Infrastructure;
using Moq;
using YeahTVApi.Entity;
using YeahTVApi.Common;
using YeahTVApi.Controllers;

namespace YeahTVApi.UnitTests.ControllerTest
{
    [TestClass]
    public class DeviceTraceControllerTest
    {
        private Mock<IDeviceTraceLibraryManager> mockTraceManager;
        private Mock<IAppLibraryManager> mockAppManager;
        private Mock<IFileUploadServiceManager> mockFileUploadServiceManager;
        private Mock<ILogManager> mockLogManager;
        private Mock<IRedisCacheManager> mockRedisCacheManager;
        private Mock<IRequestApiService> mockRequestApiService;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<IBackupDeviceManager> mockBackupDeviceManager;
        private Mock<ISysAttachmentManager> mockSysAttachmentManager;
        private Mock<IHttpContextService> mockHttpContextService;
        private DeviceTraceController deviceTraceController;
        [TestInitialize]
        public void Setup()
        {
            mockRedisCacheManager = new Mock<IRedisCacheManager>();
            mockRequestApiService = new Mock<IRequestApiService>();
            mockHttpContextService = new Mock<IHttpContextService>();
            mockLogManager = new Mock<ILogManager>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockTraceManager = new Mock<IDeviceTraceLibraryManager>();
            mockAppManager = new Mock<IAppLibraryManager>();
            mockFileUploadServiceManager = new Mock<IFileUploadServiceManager>();
            mockLogManager = new Mock<ILogManager>();
            mockRedisCacheManager = new Mock<IRedisCacheManager>();
            mockRequestApiService = new Mock<IRequestApiService>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockBackupDeviceManager = new Mock<IBackupDeviceManager>();
            mockSysAttachmentManager = new Mock<ISysAttachmentManager>();
            mockHttpContextService = new Mock<IHttpContextService>();
            var mockHeader = new RequestHeader
            {
                HotelID = "TestHotelId"
            };
            mockHttpContextService.Setup(m => m.Current.Items[RequestParameter.Header]).Returns(mockHeader);

            deviceTraceController = new DeviceTraceController(mockTraceManager.Object,
                mockAppManager.Object,
                mockBackupDeviceManager.Object,
                mockLogManager.Object,
                mockRequestApiService.Object,
                mockHttpContextService.Object,
                mockFileUploadServiceManager.Object,
                mockRedisCacheManager.Object,
                mockConstantSystemConfigManager.Object,
                mockSysAttachmentManager.Object
                );
        }
        [TestMethod]
        public void TestMethod1()
        {

        }
    }
}
