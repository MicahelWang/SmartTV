using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DataModel
{
    public class WorkStartResponse: StartResponse
    {
        public string UserName { get; set; }

        public string UserId { get; set; }

        //public Apps Apps { get; set; }
    }
}
