using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace YeahTvHcsApi.ViewModels
{
    public class PostTaskData
    {
        [JsonProperty("old_task_no")]
        public string OldTaskNo;
    }
}