using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity.CentralMapping
{
    public class CheckScratchCardResult
    {
        public int ResultCode { get; set; }
        public int Amount { get; set; }
        public int FaceValue { get; set; }
        public string ResultMsg { get; set; }
    }
}
