using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahTVApi.DomainModel.Models.YeahHcsApi
{
    public class RequestYeahApiKey
    {
        public string DeviceSeries { get; set; }

        public string PublicKey { get; set; } 
    }
}