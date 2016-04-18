using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YeahTVApiLibrary.Infrastructure;
using Moq;
using YeahTVApi.Entity;
using YeahTVApi.Common;
using YeahTVApi.Controllers;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.SearchCriteria;
using System.Linq;
using YeahTVApi.DomainModel;
using System.Collections.Generic;

namespace YeahTVApi.UnitTests.ControllerTest
{
    [TestClass]
    public class BackupDeviceTest
    {
        private Mock<IBackupDeviceManager> mockBackupdevicemanager;
        private Mock<ILogManager> mockLogManager;
        private Mock<IConstantSystemConfigManager> mockConstantSystemConfigManager;
        private Mock<IRedisCacheManager> mockRedisCacheManager;
        private Mock<IHttpContextService> mockHttpContextService;
        private Mock<IRequestApiService> mockRequestApiService;
        private BackupDeviceController backupDeviceController;
        private Mock<IDeviceTraceLibraryManager> mockTraceManager;

        [TestInitialize]
        public void Initial()
        {
            mockBackupdevicemanager = new Mock<IBackupDeviceManager>();
            mockTraceManager = new Mock<IDeviceTraceLibraryManager>();
            mockLogManager = new Mock<ILogManager>();
            mockConstantSystemConfigManager = new Mock<IConstantSystemConfigManager>();
            mockRedisCacheManager = new Mock<IRedisCacheManager>();
            mockRequestApiService = new Mock<IRequestApiService>();
            mockHttpContextService = new Mock<IHttpContextService>();
            var mockHeader = new RequestHeader
            {
                HotelID = "TestHotelId",
                DEVNO = "Test"
            };
            mockHttpContextService.Setup(m => m.Current.Items[RequestParameter.Header]).Returns(mockHeader);
            backupDeviceController = new BackupDeviceController(mockBackupdevicemanager.Object, mockLogManager.Object, mockConstantSystemConfigManager.Object, mockRequestApiService.Object, mockTraceManager.Object);
            backupDeviceController.HttpContextService = mockHttpContextService.Object;
        }

        [TestMethod]
        public void Test_AddDeviceObj_whenDeviceSeriesHave_Shouldreturnfuyi()
        {
            
            var res = new ApiResult();

            try
            {
                BackupDevice trace = new BackupDevice()
                {
                    HotelId = "0f710609748e40adba345078d6369f8b",
                    Active = true,
                    DeviceSeries = "34:80:b3:1e:99:0d",
                    DeviceType = "AIO",
                    LastUpdateTime = DateTime.Now,
                    LastUpdatUser = "admin",
                    Model = "AIO"
                };
                var exitAppversions = IntialList().Where(p=>p.DeviceSeries.Equals(trace.DeviceSeries));
                if (exitAppversions != null && exitAppversions.Any())
                {
                    res.WithError("已绑定");
                }
                else
                {
                    mockBackupdevicemanager.Object.Add(trace);
                    res.WithOk();
                }
            }
            catch (Exception ex)
            {
                res.WithError(ex.Message.ToString());
            }
            Assert.AreEqual(-1, res.ResultType);
        }

        [TestMethod]
        public void Test_AddDeviceObj_whenDeviceSeriesHave_ShouldreturnLing()
        {
            var res = new ApiResult();
            try
            {
                BackupDevice trace = new BackupDevice()
                {
                    HotelId = "0f71060974450788e40adba3d6369f8b",
                    Active = true,
                    DeviceSeries = "fsdfsfsff0d",
                    DeviceType = "AIO",
                    LastUpdateTime = DateTime.Now,
                    LastUpdatUser = "admin",
                    Model = "AIO"
                };
                var exitAppversions = IntialList().Where(p => p.DeviceSeries.Equals(trace.DeviceSeries));
                if (exitAppversions != null && exitAppversions.Any())
                {
                    res.WithError("已绑定");
                }
                else
                {
                    mockBackupdevicemanager.Object.Add(trace);
                    res.WithOk();
                }
            }
            catch (Exception ex)
            {
                res.WithError(ex.Message.ToString());
            }
            Assert.AreEqual(0, res.ResultType);
        }


        public List<BackupDevice> IntialList()
        {
            var exitAppversions = new List<BackupDevice>();
            exitAppversions.Add(new BackupDevice()
            {
                HotelId = "0f71060945078d6748e40adba3369f8b",
                Active = true,
                DeviceSeries = "34:99:80:b3:1e:0d",
                DeviceType = "AIO",
                LastUpdateTime = DateTime.Now,
                LastUpdatUser = "admin",
                Model = "AIO"
            });
            exitAppversions.Add(new BackupDevice()
            {
                HotelId = "0f710609748e40adba345078d6369f8b",
                Active = true,
                DeviceSeries = "34:80:b3:1e:99:0d",
                DeviceType = "AIO",
                LastUpdateTime = DateTime.Now,
                LastUpdatUser = "admin",
                Model = "AIO"
            });
            return exitAppversions;
        }
    }
}
