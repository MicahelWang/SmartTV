using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahOnlieShoppingMall.ViewModels
{
    /// <summary>
    /// 分类查询商品列表(分页)
    /// </summary>
    public class QueryByCategory
    {
        public QueryByCategory()
        {
        }
        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }
        [JsonProperty("pageIndex")]
        public int PageIndex { get; set; }
        [JsonProperty("pageSize")]
        public int PageSize { get; set; }
    }
    public class Category
    {
        public Category()
        {
        }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("sort")]
        public int Index { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } 
    }
}