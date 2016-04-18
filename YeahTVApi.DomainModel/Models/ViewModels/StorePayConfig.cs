
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class StorePayConfig
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplaySentRoom")]
        public System.Nullable<bool> SentRoom { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelPaymentPaymentModels")]
        public List<string> PaymentModels { get; set; }
    }
}