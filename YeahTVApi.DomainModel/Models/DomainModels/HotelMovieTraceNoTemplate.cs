using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace YeahTVApi.DomainModel.Models
{
    public partial class HotelMovieTraceNoTemplate : BaseEntity<string>
    {
        public HotelMovieTraceNoTemplate()
        {
        }
        public string HotelId { get; set; }
        public string MovieId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<double> ViewCount { get; set; }
        public string DownloadStatus { get; set; }
        public bool Active { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<bool> IsTop { get; set; }
        public System.DateTime CreateTime { get; set; }
        public System.DateTime LastViewTime { get; set; }
        public string LastUpdateUser { get; set; }
        public bool IsDelete { get; set; }

        public virtual MovieForLocalize MovieForLocalize { get; set; }
    }
}
