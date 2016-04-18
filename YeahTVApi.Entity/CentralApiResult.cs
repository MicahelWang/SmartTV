using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using YeahTVApi.Entity.CentralMapping;
namespace YeahTVApi.Entity
{
    /// <summary>
    /// 对象结果接口
    /// </summary>
    public  class CentralApiResult<T>
    {
        /// <summary>
        /// 返回对象
        /// </summary>
       public T Data { get; set; }

       /// <summary>
       ///     获取或设置 操作结果类型
       /// </summary>
       public OperationResultType ResultType { get; set; }

       /// <summary>
       ///     获取或设置 操作返回信息
       /// </summary>
       public string Message { get; set; }

       /// <summary>
       ///     获取或设置 操作返回的详细信息，用于记录日志等
       /// </summary>
       public string MoreInfo { get; set; }

       /// <summary>
       /// 系统记录日志的编号
       /// </summary>
       public string ErrorKey { get; set; }

       public string MessageID;

    }
}
