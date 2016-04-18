using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models
{
    public partial class MovieTemplateRelation : BaseEntity<string>
    {
        public string MovieId { get; set; }
        public string MovieTemplateId { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool IsFree { get; set; }
        public string LastUpdateUser { get; set; }
        public DateTime LastUpdateTime { get; set; }

        [JsonIgnore]
        public virtual Movie Movie { get; set; }

        [JsonIgnore]
        public virtual MovieTemplate MovieTemplate { get; set; }
    }
}
