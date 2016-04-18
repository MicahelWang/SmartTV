using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public class ScoreExchang : BaseEntity<string>
    {
        public ScoreExchang()
        {
            this.Id = Guid.NewGuid().ToString("N");
        }

        public string OrderType { get; set; }

        public string OrderId { get; set; }

        public string RunningNumber { get; set; }

        public int Score { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public string ScoreRate { get; set; }

        public string Productid { get; set; }

        public string Reqtime { get; set; }

        public string Remark { get; set; }     
    }
}
