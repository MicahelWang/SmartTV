using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YeahTVApi.Filter
{
 
    /// <summary>
    /// 需要传递会员ID的属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class GuestAttribute:Attribute
    {
        
    }

}