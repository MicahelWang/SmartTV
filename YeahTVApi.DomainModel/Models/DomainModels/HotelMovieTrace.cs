using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YeahTVApi.DomainModel.Models
{
    public partial class HotelMovieTrace : BaseEntity<string>
    {
        public string HotelId { get; set; }
        public string MovieId { get; set; }
        public string VideoServerAddress { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public bool IsDownload { get; set; }
        public System.DateTime LastViewTime { get; set; }
        public string MoiveTemplateId { get; set; }
        public bool Active { get; set; }
        public int Order { get; set; }

        public virtual Movie Movie { get; set; }

        public virtual MovieTemplate MovieTemplate { get; set; }
    }
}
