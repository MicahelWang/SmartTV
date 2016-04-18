using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace YeahTVApi.Entity.CentralMapping
{
    /// <summary>
    /// 发票抬头
    /// </summary>
    [Serializable]
    public class InvoiceTitle
    {
        /// <summary>
        /// 发票抬头  
        /// </summary>
        
        public String Title { get; set; }

        /// <summary>
        /// 发票内容
        /// </summary>
        
        
        public String Content { get; set; }

        /// <summary>
        /// 发票类别:0=个人  1=单位
        /// </summary>
        
        
        public int Type { get; set; }



    }
}
