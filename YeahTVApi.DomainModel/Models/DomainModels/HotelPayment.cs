using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.DomainModels
{
    public class HotelPayment
    {
        [JsonIgnore]
        public int Id { get; set; }

        [Range(typeof(decimal), "0.00", "99999999.99")]
        [RegularExpression(@"^(([0-9]+)|([0-9]+\.[0-9]{1,2}))$")]
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelPaymentPriceOfDay")]

        public decimal PriceOfDay { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelPaymentPaymentModels")]
        public List<string> PaymentModels { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelPaymentPayType")]
        public string PayType { get; set; }
    }
    public class GlobalPayment:HotelPayment
    {
        [Range(0, int.MaxValue)]
        [Display(ResourceType = typeof(Resource.Resource), Name = "Common_DisplayHotelPaymentTopMoviesCount")]
        public int TopMoviesCount { get; set; }
    }
}
