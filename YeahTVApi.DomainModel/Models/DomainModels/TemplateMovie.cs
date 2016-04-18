using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DomainModels
{
    public class TemplateVOD : TemplateTVList
    {
        public TemplateVOD()
        {
            Categories = new List<MovieApiCategoryModel>();
        }
        public List<MovieApiCategoryModel> Categories { get; set; }
        public HotelPayment HotelPayment { get; set; }
    }
}