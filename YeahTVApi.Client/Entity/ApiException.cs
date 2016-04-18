using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeahTVApi.Entity
{
    /// <summary>
    /// 错误代码枚举
    /// </summary>
    /// <remarks>
    /// System = 10
    /// Default = 100
    /// SignError = 110
    /// NotLogin = 1001
    /// </remarks>
    public enum ApiErrorType
    {
        System = 10,
        Default = 100,
        SignError = 110,
        /// <summary>
        /// 不存在的APP版本或者
        /// </summary>
        NotExistVersion = 30001,
        NotLogin = 1001,
        NotLogin1 = 1002,
        MobileIsExist=1003,
       
    }

    /// <summary>
    /// 错误接口
    /// </summary>
    public class ApiError
    {
        /// <summary>
        /// 获取错误码
        /// </summary>
        /// <param name="tp">错误类型</param>
        /// <returns>返回转换为32位的整型</returns>
        public static int GetErrorCode(ApiErrorType tp)
        {
            return (-Convert.ToInt32(tp));
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <param name="tp">错误类型</param>
        /// <returns>返回错误信息</returns>
        /// <exception>没有如下情况的时候</exception>
        /// <exception>NotLogin</exception>
        /// <exception>SignError</exception>
        /// <exception>System</exception>
        public static string GetErrorDesc(ApiErrorType tp)
        {
            string rst = "未知错误";
            if (tp == ApiErrorType.NotLogin)
            {
                rst = "使用超时，请重新登陆";
            }
            else if (tp == ApiErrorType.NotLogin1)
            {
                rst = "使用超时，请重新登陆";
            }
            else if (tp == ApiErrorType.SignError)
            {
                rst = "签名错误";
            }
            else if (tp == ApiErrorType.System)
            {
                rst = "系统错误";
            }

            return rst;
        }
    }

     /// <summary>
    /// 异常访问接口
    /// </summary>
    [Serializable]
    public class FailAccessException : Exception
    {
        /// <summary>
        /// 异常接口（信息）
        /// </summary>
        /// <param name="message">异常信息</param>
        public FailAccessException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// 异常接口
    /// </summary>
    [Serializable]
    public class ApiException : Exception
    {
        /// <summary>
        /// 异常类型
        /// </summary>
        public ApiErrorType ExceptionType { get; set; }
        /// <summary>
        /// 异常代码
        /// </summary>
        /// <remarks>获取异常类型下的异常代码</remarks>
        public int ExceptionCode
        {
            get
            {
                return ApiError.GetErrorCode(this.ExceptionType);
            }
        }

        private string message;
        /// <summary>
        /// 异常信息
        /// </summary>
        /// <remarks>异常信息为空时则返回base.Message</remarks>
        public override string Message
        {
            get
            {
                if (string.IsNullOrEmpty(message))
                {
                    return base.Message;
                }
                else
                {
                    return message;
                }
            }
        }
        /// <summary>
        /// 异常接口（信息）
        /// </summary>
        /// <param name="message">异常信息</param>
        public ApiException(string message)
            : base(message)
        {
            this.ExceptionType = ApiErrorType.Default;
        }
        /// <summary>
        /// 异常接口（类型）
        /// </summary>
        /// <param name="tp">异常类型</param>
        public ApiException(ApiErrorType tp)
            : base()
        {
            this.ExceptionType = tp;
            message = ApiError.GetErrorDesc(tp);
        }
        /// <summary>
        /// 异常接口（信息，类型）
        /// </summary>
        /// <param name="tp">异常类型</param>
        /// <param name="message">异常接口</param>
        public ApiException(ApiErrorType tp, string message)
            : base(message)
        {
            this.ExceptionType = tp;
        }
    }
}
