using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DomainModels
{
    public class HotelPermition:BaseEntity<string>
    {
        public string UserId { get; set; }
        public string PermitionType { get; set; }
        public string  TypeId { get; set; }
    }
}
