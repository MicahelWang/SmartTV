using System.Runtime.Serialization;
using System;

namespace YeahTVApi.Entity.CentralMapping
{

    /// <summary>
    /// 大众点评明细
    /// </summary>
    [Serializable]
    public partial class BusinessDetail 
    {
       

        
        public int BusinessID { get; set; }
        
        public int CategoryID { get; set; }
        
        public string BusinessName { get; set; }
        
        public string BranchName { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
        
        public string Address { get; set; }
        
        public string Telephone { get; set; }
        
        public Nullable<double> AverageRating { get; set; }
        
        public string RatingImageURL { get; set; }
        
        public string Tag { get; set; }
        
        public Nullable<int> AveragePrice { get; set; }
        
        public Nullable<int> ReviewCount { get; set; }
        
        public string BusinessURL { get; set; }
        
        public string PhotoURL { get; set; }
        
        public Nullable<bool> HasCoupon { get; set; }
        
        public Nullable<int> CouponID { get; set; }
        
        public string CouponDescription { get; set; }
        
        public string CouponURL { get; set; }
        
        public Nullable<bool> HasDeal { get; set; }
        
        public Nullable<int> DealsID { get; set; }
        
        public string DealsDescription { get; set; }
        
        public string DealsURL { get; set; }
        
        public string HotelID { get; set; }
        
        public int Distance { get; set; }
    }
}
