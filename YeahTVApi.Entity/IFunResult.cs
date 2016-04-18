using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 功能结果接口
    /// </summary>
    public interface IFunResult
    {
        /// <summary>
        /// 返回值 >=0 成功 <0 失败
        /// </summary>
        int ResultType { get; set; }

        /// <summary>
        /// 返回信息，当Code<0 时候，代表错误信息
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// True 代表包含一个成功结果 False 代表包含一个错误结果
        /// </summary>
        /// <returns></returns>
        bool isOk { get; }
    }
}
