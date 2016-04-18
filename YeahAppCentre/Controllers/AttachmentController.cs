using System;
using System.Linq;
using System.Web.Mvc;
using YeahAppCentre.Web.Utility;
using YeahTVApi.Common;
using YeahTVApi.DomainModel.Models;
using YeahTVApi.DomainModel.Models.DataModel;
using YeahTVApiLibrary.Infrastructure;

namespace YeahAppCentre.Controllers
{
    public class AttachmentController : BaseController
    {
        private readonly ISysAttachmentManager _attachmentManager;
        private readonly IQiniuCloudManager _qiniuCloudManager;
        private readonly IConstantSystemConfigManager _constantSystemConfigManager;

        public AttachmentController(ISysAttachmentManager attachmentManager,
            IQiniuCloudManager qiniuCloudManager, 
            IConstantSystemConfigManager constantSystemConfigManager)
        {
            _attachmentManager = attachmentManager;
            _qiniuCloudManager = qiniuCloudManager;
            _constantSystemConfigManager = constantSystemConfigManager;
        }

        [AllowAnonymous]
        public ActionResult Upload()
        {
            if (Request.Files.Count <= 0) return this.JsonNet("Not Found File Error", JsonRequestBehavior.AllowGet);
            var file = Request.Files.Get(0);
            if (file == null) return this.JsonNet("Not Found File Error", JsonRequestBehavior.AllowGet);
            var fileType = file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();
            if (fileType != "jpg" && fileType != "png" && fileType != "jpeg" && fileType != "mp3" && fileType != "mp4")
            {
                return this.JsonNet("ERROR:FORMAT",JsonRequestBehavior.AllowGet);
            }
            var sizeAndWith = file.ContentLength.ToSizeWithUnit();
            var filePath = _qiniuCloudManager.PutFile(file.InputStream, fileType);
           
            var model = new CoreSysAttachment
            {
                FileType = fileType,
                FileName = file.FileName,
                FilePath = filePath,
                FileSize = sizeAndWith.Item1,
                Unit = sizeAndWith.Item2,
                CrateTime = DateTime.Now
            };
            _attachmentManager.Add(model);
            model.FilePath = _constantSystemConfigManager.ResourceSiteAddress + model.FilePath;
            var json = model.ToJsonString();
            return Content(json);
        }


        [AllowAnonymous]
        public ActionResult UploadApk()
        {
            if (Request.Files.Count <= 0) return this.JsonNet("Not Found File Error", JsonRequestBehavior.AllowGet);
            var file = Request.Files.Get(0);
            if (file == null) return this.JsonNet("Not Found File Error", JsonRequestBehavior.AllowGet);
            var fileType = file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();

            var filePath = _qiniuCloudManager.PutFile(file.InputStream, fileType);
            var andriodInfo = filePath.GetAndroidInfo();
            
            var andriodObject = new AndroidInfos
            {
                Infos = andriodInfo,
                FilePath = _constantSystemConfigManager.ResourceSiteAddress + filePath
            };
            var json = andriodObject.ToJsonString();
            return Content(json);
        }

        public JsonResult Delete(int id)
        {
            _attachmentManager.Delete(id);
            return Json("Success");
        }

        public ActionResult Get(string id)
        {
            var ids = id.Split(',').Select(m => m.ToInt()).ToArray();
            var result = _attachmentManager.GetByIds(ids).Select(m => new CoreSysAttachment()
            {
                Id = m.Id,
                FileName = m.FileName,
                FilePath = _constantSystemConfigManager.ResourceSiteAddress + m.FilePath,
                FileSize = m.FileSize,
                FileType = m.FileType,
                Unit = m.Unit,
                CrateTime = m.CrateTime
            }).ToList();
            var json = result.ToJsonString();
            return Content(json);
        }
    }
}