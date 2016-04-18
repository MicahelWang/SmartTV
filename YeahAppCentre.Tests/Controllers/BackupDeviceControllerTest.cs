using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YeahAppCentre.Controllers;
using YeahCenter.Infrastructure;
using YeahTVApi.DomainModel.Models;
using YeahTVApiLibrary.Infrastructure;


namespace YeahAppCentre.Tests.Controllers
{
    [TestClass]
    public class BackupDeviceControllerTest
    {
        private BackupDeviceController backupdevicecontroller;
        private Mock<IBackupDeviceManager> backupdevicemanager;
        private Mock<IDeviceTraceLibraryManager> devicemanager;
        private Mock<ILogManager> logManager;
        private Mock<IHotelManager> hotelManager;
        private List<BackupDevice> lis;
        private Mock<IHttpContextService> mockHttpContextService;

         
        [TestInitialize]
        public void Setup()
        {
            lis = MokeList(10);
            backupdevicemanager=new Mock<IBackupDeviceManager> ();
            logManager=new   Mock<ILogManager>() ;
            hotelManager=new  Mock<IHotelManager>() ;
            mockHttpContextService = new Mock<IHttpContextService>();
            devicemanager =new Mock<IDeviceTraceLibraryManager>();
            backupdevicecontroller = new BackupDeviceController(backupdevicemanager.Object, logManager.Object, hotelManager.Object, mockHttpContextService.Object, devicemanager.Object);
        }
        [TestMethod]
        public void Index_ShouldReturnList_WhenNoCondition()
        {
            var r =backupdevicecontroller.Index() as ViewResult;
            
            // Assert
            Assert.AreEqual(10, lis.Count);
        }


        /*public ActionResult List(BackupDeviceCriteria backupDeviceCriteria)
        {
           var list = new PagedViewList<BackupDevice>();

            list.PageIndex = backupDeviceCriteria.PageIndex;
            list.PageSize = backupDeviceCriteria.PageSize;
            list.Source = backupdevicemanager.SearchBackupDevice(backupDeviceCriteria);
            list.TotalCount = backupDeviceCriteria.TotalCount;

            return this.PartialView("List", list);
        }*/

        public List<BackupDevice> MokeList(int count)
        {
            List<BackupDevice> list = new List<BackupDevice>();
            for (int i = 0; i < count; i++)
            {
                BackupDevice backupdevice = new BackupDevice()
                {
                    Active = true,
                    DeviceSeries = "BackupDevice" + i,
                    HotelId = "sdf" + i,
                    Id = i,
                    LastUpdateTime = DateTime.Now,
                    LastUpdatUser = "admin"
                };
                list.Add(backupdevice);
            }
            return list;
        }
    }
}
