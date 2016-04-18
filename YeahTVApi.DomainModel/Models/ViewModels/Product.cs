using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.DomainModel.Models.ViewModels
{
    public class Product
    {
        [JsonProperty("id")]
        public string ProductId { get; set; }
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

    }
    public class ProductList
    {
        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }
    public class ProductMinusOrAdd : Product
    {
        [JsonProperty("type")]
        public string Type
        {
            get;
            set;
        }
    }
    public enum ProductType
    {
        MINUS,
        ADD
    }
}
