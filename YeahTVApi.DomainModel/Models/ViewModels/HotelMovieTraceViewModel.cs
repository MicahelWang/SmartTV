using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class HotelMovieTraceViewModel
    {
        public string HotelId { get; set; }

        public string MoiveTemplateId { get; set; }

        public string MoiveTemplateName { get; set; }

        public string MoiveTemplateDescription { get; set; }
        public string MoiveTemplateTages { get; set; } 
    }
}
