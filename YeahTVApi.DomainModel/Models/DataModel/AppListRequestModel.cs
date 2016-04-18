using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class AppListRequestModel
    {
        public AppListRequestModel()
        {
            PackageName = "";
            VersionCode = 0;
            VersionName = "";
        }

        public string PackageName { get; set; }

        public int VersionCode { get; set; }

        public string VersionName { get; set; }
    }
}
