using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YeahTVApi.Entity;

namespace YeahTVApi.DomainModel.Models.YeahHcsApi
{
    public class RequestHcsHeader : RequestHeader
    {
        public string Product { get; set; }
    }
}