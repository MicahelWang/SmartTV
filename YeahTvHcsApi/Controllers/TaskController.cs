using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using YeahTVApiLibrary.Infrastructure;
using YeahTVApiLibrary.Infrastructure.ManagerInterface;
using YeahTVApiLibrary.Filter;
using YeahTVApi.DomainModel.Models;
using YeahTvHcsApi.ViewModels;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YeahTvHcsApi.Controllers
{
    public class TaskController : ApiController
    {
        private readonly ILogManager _logManager;
        private readonly IHCSTaskManager _taskManager;

        public TaskController(ILogManager logManager, IHCSTaskManager taskManager)
        {
            _logManager = logManager;
            _taskManager = taskManager;
        }

        [HttpPost]
        [ActionName("AllTask")]
        [HCSCheckSignFilter(GetPrivateKey = false, NeedCheckSign = true)]
        public ResponseData<List<HCSTask>> PostAllTask(PostParameters<PostTaskData> request)
        {
            List<HCSTask> task = _taskManager.GetTask(request.Server_Id, request.Sign, request.Data.OldTaskNo);

            return new ResponseData<List<HCSTask>> { Sign = "", Data = task };
        }

        [HttpPost]
        [ActionName("TaskStatusNotify")]
        [HCSCheckSignFilter(GetPrivateKey = false, NeedCheckSign = false)]
        public void PostTaskStatusNotify(PostParameters<PostTaskStatusNotifyData> request)
        {
            PostTaskStatusNotifyData postData = request.Data;

            _taskManager.UpdateTaskStatus(request.Server_Id, request.Sign, postData.BizType, postData.BizNo, postData.Status, postData.ErrorMessage);
        }
    }
}